﻿using System;
using System.Data;
using Shuttle.Abacus.DataAccess;
using Shuttle.Abacus.Infrastructure;
using Shuttle.Abacus.Localisation;
using Shuttle.Abacus.UI.Coordinators.Interfaces;
using Shuttle.Abacus.UI.Core.Presentation;
using Shuttle.Abacus.UI.Core.Resources;
using Shuttle.Abacus.UI.Messages.Core;
using Shuttle.Abacus.UI.Messages.Explorer;
using Shuttle.Abacus.UI.Messages.Resources;
using Shuttle.Abacus.UI.Messages.TestCase;
using Shuttle.Abacus.UI.Models;
using Shuttle.Abacus.UI.UI.List;
using Shuttle.Abacus.UI.UI.MethodTest;
using Shuttle.Abacus.UI.UI.MethodTest.Results;
using Shuttle.Abacus.UI.UI.Shell.TabbedWorkspace;
using Shuttle.Abacus.UI.UI.WorkItem.ContextToolbar;
using Shuttle.Abacus.UI.WorkItemControllers.Interfaces;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Abacus.UI.Coordinators
{
    public class TestCoordinator :
        Coordinator,
        ITestCoordinator
    {
        private readonly IArgumentQuery _argumentQuery;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IMethodTestQuery _testQuery;

        public TestCoordinator(IDatabaseContextFactory databaseContextFactory, IMethodTestQuery testQuery,
            IArgumentQuery argumentQuery)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(testQuery, "testQuery");
            Guard.AgainstNull(argumentQuery, "argumentQuery");

            _databaseContextFactory = databaseContextFactory;
            _argumentQuery = argumentQuery;
            _testQuery = testQuery;
        }

        public void HandleMessage(ManageMethodTestsMessage message)
        {
            var presenter = WorkItemManager.BuildPresenter<ISimpleListPresenter>()
                .AddNavigationItem(NavigationItemFactory.Create<PrintMethodTestMessage>())
                .AddNavigationItem(NavigationItemFactory.Create<RunMethodTestMessage>())
                .AddNavigationItem(NavigationItemFactory.Create(new RemoveMethodTestMessage(message.MethodId)))
                .AddNavigationItem(NavigationItemFactory.Create<EditMethodTestMessage>())
                .AddNavigationItem(NavigationItemFactory.Create(new NewMethodTestMessage(message.MethodId)))
                .AddNavigationItem(NavigationItemFactory.Create(new NewMethodTestFromExistingMessage(message.MethodId)))
                .AddNavigationItem(NavigationItemFactory.Create<MarkAllMessage>())
                .AddNavigationItem(NavigationItemFactory.Create<InvertMarksMessage>());

            var item = WorkItemManager
                .Create($"Tests: {message.MethodName}")
                .ControlledBy<IMethodTestManagerController>()
                .ShowIn<IContextToolbarPresenter>()
                .AddPresenter(presenter)
                .WithModel(new SimpleListModel("MethodTestId", _testQuery.FetchForMethodId(message.MethodId)))
                .AddPresenter<IMethodTestResultPresenter>()
                .AssignInitiator(message);

            HostInWorkspace<ITabbedWorkspacePresenter>(item);
        }

        public void HandleMessage(ResourceMenuRequestMessage message)
        {
            if (!message.Item.ResourceKey.Equals(ResourceKeys.Test))
            {
                return;
            }

            switch (message.Item.Type)
            {
                case Resource.ResourceType.Container:
                {
                    message.NavigationItems.Add(
                        NavigationItemFactory.Create(
                            new ManageMethodTestsMessage(message.Item.Key, message.Item.Text)));

                    break;
                }
                case Resource.ResourceType.Item:
                {
                    break;
                }
            }
        }

        public void HandleMessage(NewMethodTestMessage message)
        {
            var item = WorkItemManager
                .Create("New test case")
                .ControlledBy<IMethodTestController>()
                .ShowIn<IContextToolbarPresenter>()
                .AddPresenter<IMethodTestPresenter>()
                .WithModel(BuildModel())
                .AddNavigationItem(NavigationItemFactory.Create(message).AssignResourceItem(ResourceItems.Submit))
                .AsDefault()
                .AssignInitiator(message);

            HostInWorkspace<ITabbedWorkspacePresenter>(item);
        }

        public void HandleMessage(EditMethodTestMessage message)
        {
            var model = BuildModel();

            DataRow row;

            using (_databaseContextFactory.Create())
            {
                row = _testQuery.Get(message.MethodTestId);
            }

            model.MethodTestRow = row;
            model.ArgumentAnswers = _testQuery.GetArgumentAnswers(message.MethodTestId).CopyToDataTable();

            message.MethodId = new Guid(row["MethodId"].ToString());
            message.Description = MethodTestColumns.Description.MapFrom(row);
            message.ExpectedResult = MethodTestColumns.ExpectedResult.MapFrom(row);

            var item = WorkItemManager
                .Create("Test: " + MethodTestColumns.Description.MapFrom(row))
                .ControlledBy<IMethodTestController>()
                .ShowIn<IContextToolbarPresenter>()
                .AddPresenter<IMethodTestPresenter>()
                .WithModel(model)
                .AddNavigationItem(NavigationItemFactory.Create(new ChangeMethodTestMessage(message)))
                .AsDefault()
                .AssignInitiator(message);

            HostInWorkspace<ITabbedWorkspacePresenter>(item);
        }

        public void HandleMessage(MethodTestCreatedMessage message)
        {
            RefreshList(message.WorkItemId, message.MethodId);
        }

        public void HandleMessage(MethodTestChangedMessage message)
        {
            RefreshList(message.WorkItemId, message.MethodId);
        }

        public void HandleMessage(MethodTestRemovedMessage message)
        {
            RefreshList(message.WorkItemId, message.MethodId);
        }

        public void HandleMessage(MethodTestRunMessage message)
        {
            var workItem = WorkItemManager.Get(message.WorkItemId);

            if (workItem == null)
            {
                return;
            }

            var list = workItem.GetView<ISimpleListView>();

            if (!list.Contains(message.Event.MethodTestId))
            {
                return;
            }

            var view = workItem.GetView<IMethodTestResultView>();

            view.AddRun(
                message.Event.MethodTestId,
                message.Event.MethodTestDescription,
                message.Event.ExpectedResult);

            view.ShowView();
        }

        public void HandleMessage(NewMethodTestFromExistingMessage message)
        {
            var model = BuildModel();

            DataRow row;

            using (_databaseContextFactory.Create())
            {
                row = _testQuery.Get(message.MethodTestId);
            }

            model.MethodTestRow = row;
            model.ArgumentAnswers = _testQuery.GetArgumentAnswers(message.MethodTestId).CopyToDataTable();

            var item = WorkItemManager
                .Create(string.Format("New test case from '{0}'", MethodTestColumns.Description.MapFrom(row)))
                .ControlledBy<IMethodTestController>()
                .ShowIn<IContextToolbarPresenter>()
                .AddPresenter<IMethodTestPresenter>()
                .WithModel(model)
                .AddNavigationItem(
                    NavigationItemFactory.Create(new NewMethodTestMessage(message))
                        .AssignResourceItem(ResourceItems.Submit))
                .AsDefault()
                .AssignInitiator(message);

            HostInWorkspace<ITabbedWorkspacePresenter>(item);
        }

        public void HandleMessage(SummaryViewRequestedMessage message)
        {
            if (SummaryViewManager.CanIgnore(message, ResourceKeys.Test))
            {
                return;
            }

            using (_databaseContextFactory.Create())
            {
                switch (message.Item.Type)
                {
                    case Resource.ResourceType.Container:
                    {
                        message.AddTable("Test Cases", _testQuery.All());

                        break;
                    }
                    case Resource.ResourceType.Item:
                    {
                        break;
                    }
                }
            }
        }

        private MethodTestModel BuildModel()
        {
            using (_databaseContextFactory.Create())
            {
                return new MethodTestModel
                {
                    ArgumentRows = _argumentQuery.All()
                };
            }
        }

        private void RefreshList(Guid workItemId, Guid methodId)
        {
            var workItem = WorkItemManager.Get(workItemId);

            if (workItem == null)
            {
                return;
            }

            var presenter = workItem.GetPresenter<ISimpleListPresenter>();

            using (_databaseContextFactory.Create())
            {
                var modelPresenter = presenter as IPresenter<SimpleListModel>;

                if (modelPresenter == null)
                {
                    throw new InvalidOperationException();
                }

                modelPresenter.AssignModel(new SimpleListModel("MethodTestId", _testQuery.FetchForMethodId(methodId)));
            }

            presenter.Refresh();
        }

        public void HandleMessage(ExplorerInitializeMessage message)
        {
            if (!Permissions.Test.IsSatisfiedBy(Session.Permissions))
            {
                return;
            }

            message.Items.Add(
                new Resource(ResourceKeys.Test, Guid.NewGuid(), "Tests", ImageResources.Test)
                    .AsContainer());
        }
    }
}