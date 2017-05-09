using Shuttle.Abacus.Invariants.Core;
using Shuttle.Abacus.Invariants.Interfaces;

namespace Shuttle.Abacus.Invariants
{
    public class ArgumentRules : IArgumentRules
    {
        public IRuleCollection<object> ArgumentNameRules()
        {
            return Rule.With().Required().MaximumLength(120).Create();
        }

        public IRuleCollection<object> ValueTypeRules()
        {
            return Rule.With().Required().Create();
        }

    }
}
