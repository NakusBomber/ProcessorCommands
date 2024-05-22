using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Helpers.Validations
{
    public class DecimalValidation : IValidateValue
    {
        public List<string> Validate(string value)
        {
            var errors = new List<string>();

            if (value == string.Empty)
                return errors;

            if (!int.TryParse(value, out int intValue))
                errors.Add(Resources.General.LettersNotAllowed);

            if (intValue < 0 || intValue > 255)
            {
                errors.Add($"{Resources.General.OnlyByteValues} (0 - 255)");
            }

            if (intValue > 0 && value.StartsWith("0"))
                errors.Add($"{Resources.General.EntryMustNotStartWith} 0");

            return errors;
        }
    }
}
