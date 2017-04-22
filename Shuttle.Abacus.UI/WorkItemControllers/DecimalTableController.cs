using Shuttle.Abacus.UI.Core.WorkItem;
using Shuttle.Abacus.UI.Messages.DecimalTable;
using Shuttle.Abacus.UI.Messages.WorkItem;
using Shuttle.Abacus.UI.UI.DecimalTable;
using Shuttle.Abacus.UI.WorkItemControllers.Interfaces;

namespace Shuttle.Abacus.UI.WorkItemControllers
{
    public class DecimalTableController : WorkItemController, IDecimalTableController
    {
        public void HandleMessage(NewDecimalTableMessage message)
        {
            if (!WorkItem.PresentationValid())
            {
                return;
            }

            var view = WorkItem.GetView<IDecimalTableView>();

            if (view.HasInvalidDecimalTable())
            {
                WorkItem.GetPresenter<IDecimalTablePresenter>().ShowInvalidDecimalTableMessage();

                return;
            }

            var command = new CreateDecimalTableCommand
                          {
                              DecimalTableName = view.DecimalTableNameValue,
                              RowArgumentDto = view.RowArgumentDto,
                              ColumnArgumentDTO = view.ColumnArgumentDTO,
                              DecimalValueDTOs = view.DecimalValueDTOs()
                          };

            Send(command);
        }

        public void HandleMessage(EditDecimalTableMessage message)
        {
            if (!WorkItem.PresentationValid())
            {
                return;
            }

            var view = WorkItem.GetView<IDecimalTableView>();

            if (view.HasInvalidDecimalTable())
            {
                WorkItem.GetPresenter<IDecimalTablePresenter>().ShowInvalidDecimalTableMessage();

                return;
            }

            var command = new UpdateDecimalTableCommand
                          {
                              DecimalTableId = message.DecimalTableId,
                              DecimalTableName = view.DecimalTableNameValue,
                              RowArgumentDto = view.RowArgumentDto,
                              ColumnArgumentDTO = view.ColumnArgumentDTO,
                              DecimalValueDTOs = view.DecimalValueDTOs()
                          };

            Send(command,
                 () =>
                 _messageBus.Publish(new RefreshWorkItemDispatcherTextMessage(WorkItem.Initiator.WorkItemInitiatorId)));
        }
    }
}