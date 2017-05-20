using Shuttle.Abacus.Shell.Core.Messaging;
using Shuttle.Abacus.Shell.Messages.Formula;
using Shuttle.Abacus.Shell.Messages.Resources;

namespace Shuttle.Abacus.Shell.Coordinators
{
    public interface IFormulaConstraintCoordinator :
        IMessageHandler<PopulateResourceMessage>,
        IMessageHandler<ResourceMenuRequestMessage>,
        IMessageHandler<ManageFormulaConstraintsMessage>
    {
    }
}