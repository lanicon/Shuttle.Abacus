using System.Collections.Generic;

namespace Shuttle.Abacus.UI.Models
{
    public class ManageFormulaConstraintsModel
    {
        public List<string> ConstraintTypes { get; set; }
        public List<ArgumentModel> Arguments { get; set; }
        public List<FormulaConstraintModel> Constraints { get; set; }
    }
}