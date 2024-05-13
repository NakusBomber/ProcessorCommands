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
                errors.Add("Только 0 и 1 (бинарный ввод, начиная с \"0b\")");

            var notEnough = "Не хватает";
            var moreThan = "Превышено на";
            if (value.Length != 10)
            {
                var info = value.Length < 10 ? notEnough : moreThan;
                
                errors.Add($"Длина должна быть 8 бит ({info}: {Math.Abs(value.Length-10)})");
            }
                
            return errors;
        }
    }
}
