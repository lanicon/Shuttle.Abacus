using System;
using Shuttle.Abacus.UI.Core.Messaging;

namespace Shuttle.Abacus.UI.Messages.Section
{
    public class EditMethodMessage : Message
    {
        public Guid MethodId;

        public EditMethodMessage(Guid methodId)
        {
            MethodId = methodId;
        }

        public override IPermission RequiredPermission
        {
            get { return Permissions.Method; }
        }
    }
}