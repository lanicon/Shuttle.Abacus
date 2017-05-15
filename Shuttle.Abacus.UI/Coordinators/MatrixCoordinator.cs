using System;
using System.Data;
using Shuttle.Abacus.DataAccess;
using Shuttle.Abacus.Infrastructure;
using Shuttle.Abacus.Localisation;
using Shuttle.Abacus.UI.Coordinators.Interfaces;
using Shuttle.Abacus.UI.Core.Presentation;
using Shuttle.Abacus.UI.Core.Resources;
using Shuttle.Abacus.UI.Messages.Core;
using Shuttle.Abacus.UI.Messages.DecimalTable;
using Shuttle.Abacus.UI.Messages.Explorer;
using Shuttle.Abacus.UI.Messages.Report;
using Shuttle.Abacus.UI.Messages.Resources;
using Shuttle.Abacus.UI.Models;
using Shuttle.Abacus.UI.UI.Matrix;
using Shuttle.Abacus.UI.UI.Shell.TabbedWorkspace;
using Shuttle.Abacus.UI.UI.WorkItem.ContextToolbar;
using Shuttle.Abacus.UI.WorkItemControllers.Interfaces;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Abacus.UI.Coordinators
{
    public class MatrixCoordinator : Coordinator, IMatrixCoordinator
    {
        private readonly IArgumentQuery _argumentQuery;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IMatrixQuery _matrixQuery;

        public MatrixCoordinator(IDatabaseContextFactory databaseContextFactory, IArgumentQuery argumentQuery,
            IMatrixQuery matrixQuery)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(argumentQuery, "argumentQuery");
            Guard.AgainstNull(matrixQuery, "matrixQuery");

            _databaseContextFactory = databaseContextFactory;
            _argumentQuery = argumentQuery;
            _matrixQuery = matrixQuery;
        }

        public void HandleMessage(ExplorerInitializeMessage message)
        {
            if (!Permissions.Matrix.IsSatisfiedBy(Session.Permissions))
            {
                return;
            }

            message.Items.Add(
                new Resource(ResourceKeys.Matrix, Guid.NewGuid(), "Matrixes", ImageResources.Matrix)
                    .AsContainer());
        }

        public void HandleMessage(ResourceMenuRequestMessage message)
        {
            if (!message.Item.ResourceKey.Equals(ResourceKeys.Matrix))
            {
                return;
            }

            switch (message.Item.Type)
            {
                case Resource.ResourceType.Container:
                {
                    message.NavigationItems.Add(NavigationItemFactory.Create<NewMatrixMessage>());

                    break;
                }
                case Resource.ResourceType.Item:
                {
                    message.NavigationItems.Add(
                        NavigationItemFactory.Create(
                            new NewDecimalTableFromExistingMessage(message.Item.Key)));

                    message.NavigationItems.Add(
                        NavigationItemFactory.Create(
                            new EditMatrixMessage(message.Item.Key,
                                message.Item.Text)));

                    message.NavigationItems.Add(
                        NavigationItemFactory.Create(
                            new MatrixReportMessage(message.Item.Key,
                                message.Item.Text)));

                    break;
                }
            }
        }

        public void HandleMessage(PopulateResourceMessage message)
        {
            if (!message.Resource.ResourceKey.Equals(ResourceKeys.Matrix))
            {
                return;
            }

            using (_databaseContextFactory.Create())
            {
                switch (message.Resource.Type)
                {
                    case Resource.ResourceType.Container:
                    {
                        foreach (
                            var row in
                            _matrixQuery.All())
                        {
                            message.Resources.Add(new Resource(ResourceKeys.Matrix,
                                MatrixColumns.Id.MapFrom(row),
                                MatrixColumns.Name.MapFrom(row),
                                ImageResources.Matrix).AsLeaf());
                        }

                        break;
                    }
                }
            }
        }

        public void HandleMessage(ResourceRefreshItemTextMessage message)
        {
            if (!message.Item.ResourceKey.Equals(ResourceKeys.Matrix) ||
                                                message.Item.Type != Resource.ResourceType.Item)
            {
                return;
            }

            using (_databaseContextFactory.Create())
            {
                message.Item.AssignText(MatrixColumns.Name.MapFrom(_matrixQuery.Get(message.Item.Key)));
            }
        }

        public void HandleMessage(NewMatrixMessage message)
        {
            var item = WorkItemManager
                .Create("New Decimal Table")
                .ControlledBy<IMatrixController>()
                .ShowIn<IContextToolbarPresenter>()
                .AddPresenter<IMatrixPresenter>()
                .AddNavigationItem(NavigationItemFactory.Create(message).WithResourceItem(ResourceItems.Submit)).
                AsDefault()
                .AssignInitiator(message);

            HostInWorkspace<ITabbedWorkspacePresenter>(item);
        }

        public void HandleMessage(EditMatrixMessage message)
        {
            using (_databaseContextFactory.Create())
            {
                var item = WorkItemManager
                    .Create(string.Format("Decimal Table: {0}", message.DecimalTableName))
                    .ControlledBy<IMatrixController>()
                    .ShowIn<IContextToolbarPresenter>()
                    .AddPresenter<IMatrixPresenter>()
                    .WithModel(new MatrixModel(_matrixQuery.Get(message.MatrixId)))
                    .AddNavigationItem(NavigationItemFactory.Create(message).WithResourceItem(ResourceItems.Submit))
                    .AsDefault()
                    .AssignInitiator(message);

                HostInWorkspace<ITabbedWorkspacePresenter>(item);
            }
        }

        public void HandleMessage(NewDecimalTableFromExistingMessage message)
        {
            using (_databaseContextFactory.Create())
            {
                var item = WorkItemManager
                    .Create("New Decimal Table")
                    .ControlledBy<IMatrixController>()
                    .ShowIn<IContextToolbarPresenter>()
                    .AddPresenter<IMatrixPresenter>()
                    .WithModel(new MatrixModel(_matrixQuery.Get(message.MatrixId)))
                    .AddNavigationItem(
                        NavigationItemFactory.Create<NewMatrixMessage>()
                            .WithResourceItem(
                                ResourceItems.Submit))
                    .AsDefault()
                    .AssignInitiator(message.WithRefreshOwner());

                HostInWorkspace<ITabbedWorkspacePresenter>(item);
            }
        }

        public void HandleMessage(SummaryViewRequestedMessage message)
        {
            if (SummaryViewManager.CanIgnore(message, ResourceKeys.Matrix))
            {
                return;
            }

            using (_databaseContextFactory.Create())
            {
                switch (message.Item.Type)
                {
                    case Resource.ResourceType.Container:
                    {
                        message.AddTable("DecimalTables", _matrixQuery.All());

                        break;
                    }
                    case Resource.ResourceType.Item:
                    {
                        break;
                    }
                }
            }
        }
    }
}