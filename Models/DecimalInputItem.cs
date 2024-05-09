using ProcessorCommands.Helpers.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProcessorCommands.Models
{
    public class DecimalInputItem : InputItemDataGrid
    {
        public DecimalInputItem(string label, string value) : base(label, value)
        {
            Validation = new DecimalValidation();
        }

        protected override IValidateValue Validation { get; set; }
    }
}
