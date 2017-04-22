using System;
using Shuttle.Abacus.Localisation;

namespace Shuttle.Abacus.Invariants.Core
{
    public class RequiredRule : Rule
    {
        public RequiredRule()
            : base(Resources.RequiredRule, (item, rule) => Convert.ToString(item).Length < 1)
        {
        }
    }
}