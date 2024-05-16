using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProcessorCommands.Helpers.Validations
{
    public class BinaryValidation : IValidateValue
    {
        public List<string> Validate(string value)
        {
            
            if (value == string.Empty)
                return new List<string>();

            var errors = new List<string>();
            Regex regex = new Regex(@"^0b[01]+$");

            if (!regex.IsMatch(value))
                errors.Add(Resources.General.BinaryOnlyZeroOne);

            var notEnough = Resources.General.BinaryNotEnough;
            var moreThan = Resources.General.BinaryMoreThan;
            if (value.Length != 10)
            {
                var info = value.Length < 10 ? notEnough : moreThan;
                
                errors.Add($"{Resources.General.BinaryLengthShould} ({info}: {Math.Abs(value.Length-10)})");
            }
                
            return errors;
        }
    }
}
