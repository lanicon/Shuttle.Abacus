using System;
using Shuttle.Abacus.Shell.Messages.Core;

namespace Shuttle.Abacus.Shell.Messages.Test
{
    public class TestChangedMessage : NullPermissionMessage
    {
        public TestChangedMessage(Guid workItemId, Guid methodId)
        {
            WorkItemId = workItemId;
            MethodId = methodId;
        }

        public Guid WorkItemId { get; private set; }
        public Guid MethodId { get; private set; }
    }
}
