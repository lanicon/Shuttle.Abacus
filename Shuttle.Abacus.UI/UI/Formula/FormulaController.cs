using System.Collections.Generic;
using Shuttle.Abacus.Messages.v1;
using Shuttle.Abacus.Shell.Core.Messaging;
using Shuttle.Abacus.Shell.Core.WorkItem;
using Shuttle.Abacus.Shell.Messages.Formula;
using Shuttle.Abacus.Shell.UI.Formula.FormulaConstraint;
using Shuttle.Abacus.Shell.UI.Formula.FormulaOperation;
using Shuttle.Esb;

namespace Shuttle.Abacus.Shell.UI.Formula
{
    public class FormulaController : WorkItemController, IFormulaController
    {
        public FormulaController(IServiceBus serviceBus, IMessageBus messageBus)
            : base(serviceBus, messageBus)
        {
        }

        public void HandleMessage(RegisterFormulaMessage message)
        {
            if (!WorkItem.PresentationValid())
            {
                return;
            }

            var formulaView = WorkItem.GetView<IFormulaView>();

            Send(new RegisterFormulaCommand
            {
                Name = formulaView.NameValue
            });
        }


        public void HandleMessage(RenameFormulaMessage message)
        {
            if (!WorkItem.PresentationValid())
            {
                return;
            }

            var formulaView = WorkItem.GetView<IFormulaView>();

            Send(new RenameFormulaCommand
            {
                FormulaId = message.FormulaId,
                Name = formulaView.NameValue
            });
        }

        public void HandleMessage(RemoveFormulaMessage message)
        {
            Send(new RemoveFormulaCommand
            {
                FormulaId = message.FormulaId
            });
        }

        public void HandleMessage(ManageFormulaOperationsMessage message)
        {
            if (!WorkItem.PresentationValid())
            {
                return;
            }

            var view = WorkItem.GetView<IFormulaOperationView>();

            var operations = new List<Abacus.Messages.v1.TransferObjects.FormulaOperation>();

            var sequenceNumber = 1;

            foreach (var model in view.FormulaOperations)
            {
                operations.Add(new Abacus.Messages.v1.TransferObjects.FormulaOperation
                {
                    SequenceNumber = sequenceNumber++,
                    Operation = model.Operation,
                    ValueSelection = model.ValueSelection,
                    ValueSource = model.ValueSource
                });
            }

            Send(new SetFormulaOperationsCommand
            {
                FormulaId = message.FormulaId,
                Operations = operations
            });
        }

        public void HandleMessage(ManageFormulaConstraintsMessage message)
        {
            if (!WorkItem.PresentationValid())
            {
                return;
            }

            var view = WorkItem.GetView<IFormulaConstraintView>();

            var constraints = new List<Abacus.Messages.v1.TransferObjects.FormulaConstraint>();

            var sequenceNumber = 1;

            foreach (var model in view.FormulaConstraints)
            {
                constraints.Add(new Abacus.Messages.v1.TransferObjects.FormulaConstraint
                {
                    SequenceNumber = sequenceNumber++,
                    ArgumentName = model.ArgumentName,
                    Comparison = model.Comparison,
                    Value = model.Value
                });
            }

            Send(new SetFormulaConstraintsCommand
            {
                FormulaId = message.FormulaId,
                Constraints = constraints
            });
        }
    }
}