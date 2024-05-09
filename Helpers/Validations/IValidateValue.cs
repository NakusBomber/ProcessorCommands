using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Helpers.Validations
{
    public interface IValidateValue
    {
        List<string> Validate(string value);
    }
}
