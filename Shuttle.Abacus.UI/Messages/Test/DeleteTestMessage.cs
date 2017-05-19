using System;
using Shuttle.Abacus.Infrastructure;
using Shuttle.Abacus.Shell.Core.Messaging;

namespace Shuttle.Abacus.Shell.Messages.Test
{
    public class DeleteTestMessage : Message
    {
        public override IPermission RequiredPermission => Permissions.Test;

        public Guid MethodTestId { get; set; }
        public Guid MethodId { get; set; }
        public string Description { get; set; }
        public string ExpectedResult { get; set; }
    }
}
