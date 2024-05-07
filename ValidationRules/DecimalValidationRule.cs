using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProcessorCommands.ValidationRules
{
    public class DecimalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value.ToString() == string.Empty)
                return ValidationResult.ValidResult;

            if (!int.TryParse(value.ToString(), out int intValue))
                return new ValidationResult(false, "sdascxzc");

            if (intValue < 0 || intValue > 255)
                return new ValidationResult(false, "cscsadsdas");

            return ValidationResult.ValidResult;
        }
    }
}
