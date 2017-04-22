using System;
using Shuttle.Abacus.Localisation;

namespace Shuttle.Abacus.Invariants.Core.Rules
{
    public class IntegerRule : Rule
    {
        public IntegerRule()
            : base(Resources.IntegerRule, (item, rule) =>
                {
                    int i;

                    return !int.TryParse(Convert.ToString(item), out i);
                })
        {
        }
    }
}