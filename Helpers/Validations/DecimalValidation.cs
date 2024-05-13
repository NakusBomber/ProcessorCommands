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
                errors.Add("Буквы не допускаются");

            if (intValue < 0 || intValue > 255)
                errors.Add("Только байтовые значения (0 - 255)");

            if (intValue > 0 && value.StartsWith("0"))
                errors.Add("Ввод не должен начинаться с 0");

            return errors;
        }
    }
}
