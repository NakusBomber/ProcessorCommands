using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProcessorCommands.Helpers.Validations
{
    public class BinaryDecimalValidation : IValidateValue
    {
        public List<string> Validate(string value)
        {
            if (value == string.Empty)
                return new List<string>();

            if(value.StartsWith("0b"))
                return new BinaryValidation().Validate(value);

            return new DecimalValidation().Validate(value);
        }
    }
}
