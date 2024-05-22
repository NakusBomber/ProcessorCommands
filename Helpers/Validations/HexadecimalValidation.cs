using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProcessorCommands.Helpers.Validations
{
    public class HexadecimalValidation : IValidateValue
    {
        public List<string> Validate(string value)
        {
            if (value == string.Empty)
                return new List<string>();

            var errors = new List<string>();
            var regex = new Regex(@"^0x[0-9A-F]+$");

            if (!regex.IsMatch(value))
            {
                errors.Add(Resources.General.OnlyHexLettersAndNumbers);
            }
            else
            {
                var intValue = Convert.ToInt32(value, 16);
                if (intValue < 0 || intValue > 255)
                    errors.Add(Resources.General.NumberMustBeByte);
            }

            
            return errors;
        }
    }
}
