using System.Collections.Generic;
using Shuttle.Abacus.Messages.v1;
using Shuttle.Abacus.Shell.Core.Messaging;
using Shuttle.Abacus.Shell.Core.WorkItem;
using Shuttle.Abacus.Shell.Messages.SystemUser;
using Shuttle.Esb;

namespace Shuttle.Abacus.Shell.UI.SystemUser
{
    public class SystemUserController : WorkItemController, ISystemUserController
    {
        public SystemUserController(IServiceBus serviceBus, IMessageBus messageBus) 
            : base(serviceBus, messageBus)
        {
        }

        public void HandleMessage(NewSystemUserMessage message)
        {
            if (!WorkItem.PresentationValid())
            {
                return;
            }

            var systemUserView = WorkItem.GetView<ISystemUserView>();
            var permissionsView = WorkItem.GetView<IPermissionsView>();

            var permissions = new List<string>();

            permissionsView.AssignedPermissions.ForEach(permission => permissions.Add(permission.Identifier));

            var command = new CreateSystemUserCommand
                              {
                                  LoginName = systemUserView.LoginNameValue,
                                  PermissionIdentifiers = permissions
                              };

            Send(command);
        }

        public void HandleMessage(EditPermissionsMessage message)
        {
            var permissionsView = WorkItem.GetView<IPermissionsView>();

            var permissions = new List<string>();

            permissionsView.AssignedPermissions.ForEach(permission => permissions.Add(permission.Identifier));

            Send(new SetPermissionsCommand
            {
                SystemUserId = message.SystemUserId,
                PermissionIdentifiers = permissions
            });
        }

        public void HandleMessage(EditLoginNameMessage message)
        {
            var systemUserView = WorkItem.GetView<ISystemUserView>();

            Send(new ChangeLoginNameCommand
            {
                SystemUserId = message.SystemUserId,
                NewLoginName = systemUserView.LoginNameValue
            });
        }
    }
}
