using System;
using Shuttle.Abacus.UI.Core.WorkItem;
using Shuttle.Abacus.UI.Messages.Core;
using Shuttle.Abacus.UI.Messages.Formula;
using Shuttle.Abacus.UI.Messages.Resources;
using Shuttle.Abacus.UI.Messages.WorkItem;
using Shuttle.Abacus.UI.UI.Constraint;
using Shuttle.Abacus.UI.UI.Formula;
using Shuttle.Abacus.UI.UI.List;
using Shuttle.Abacus.UI.WorkItemControllers.Interfaces;

namespace Shuttle.Abacus.UI.WorkItemControllers
{
    public class FormulaController : WorkItemController, IFormulaController
    {
        public void HandleMessage(NewFormulaMessage message)
        {
            if (!WorkItem.PresentationValid())
            {
                return;
            }

            var formulaView = WorkItem.GetView<IFormulaView>();
            var constraintView = WorkItem.GetView<IConstraintView>();

            Send(new CreateFormulaCommand
                 {
                     OwnerName = message.OwnerName,
                     OwnerId = message.OwnerId,
                     Operations = formulaView.Operations,
                     Constraints = constraintView.Constraints
                 });
        }

        public void HandleMessage(EditFormulaMessage message)
        {
            if (!WorkItem.PresentationValid())
            {
                return;
            }

            var formulaView = WorkItem.GetView<IFormulaView>();
            var constraintView = WorkItem.GetView<IConstraintView>();

            Send(new ChangeFormulaCommand
                 {
                     CalculationId = message.OwnerId,
                     FormulaId = message.FormulaId,
                     Operations = formulaView.Operations,
                     Constraints = constraintView.Constraints
                 }, () =>
                    _messageBus.Publish(new RefreshWorkItemDispatcherTextMessage(WorkItem.Initiator.WorkItemInitiatorId)));
        }

        public void HandleMessage(MoveUpMessage message)
        {
            var view = WorkItem.GetView<ISimpleListView>();

            view.MoveUp(view.SelectedItem());
        }

        public void HandleMessage(MoveDownMessage message)
        {
            var view = WorkItem.GetView<ISimpleListView>();

            view.MoveDown(view.SelectedItem());
        }

        public void HandleMessage(ChangeFormulaOrderMessage message)
        {
            var view = WorkItem.GetView<ISimpleListView>();

            var command = new ChangeFormulaOrderCommand
                          {
                              OwnerId = message.OwnerId,
                              OwnerName = message.OwnerName,
                          };

            foreach (var item in view.Items)
            {
                command.OrderedIds.Add(new Guid(item.Name));
            }

            Send(command);
        }

        public void HandleMessage(DeleteFormulaMessage message)
        {
            Send(new DeleteFormulaCommand
                 {
                     FormulaId = message.FormulaId
                 },
                 () => _messageBus.Publish(new ResourceRefreshItemMessage(message.OwnerResource)));
        }
    }
}