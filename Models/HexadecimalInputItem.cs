using ProcessorCommands.Helpers.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models
{
    public class HexadecimalInputItem : InputItem
    {
        public HexadecimalInputItem(string label, string value) : base(label, value)
        {
            Validation = new HexadecimalValidation();
        }

    }
}
