using ProcessorCommands.Helpers.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProcessorCommands.Models
{
    public class DecimalInputItem : InputItem
    {
        public DecimalInputItem(string label = null, string value = null) : base(label, value)
        {
            Validation = new DecimalValidation();
        }

    }
}
