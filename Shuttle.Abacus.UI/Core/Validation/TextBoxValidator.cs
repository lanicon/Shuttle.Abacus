using System.Windows.Forms;

namespace Shuttle.Abacus.UI.Core.Validation
{
    public class TextBoxValidator : ControlValidator<TextBox>, IControlValidator
    {
        public TextBoxValidator(IValidationConfiguration validationConfiguration) : base(validationConfiguration)
        {
        }

        public IResult Validate(Control control, IRuleCollection<object> rules)
        {
            return rules.BrokenBy(control.Text).ToResult();
        }

        public void WireValidationRequired(Control control, IControlValidation validation)
        {
            var textbox = (TextBox) control;

            textbox.TextChanged +=
                delegate
                    {
                        textbox.BackColor = !validation.Evaluate().OK
                                                ? ValidationConfiguration.ErrorColor
                                                : validation.OriginalControlColor;
                    };
        }
    }
}