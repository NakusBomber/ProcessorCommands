using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models
{
    public class InputItemDataGrid
    {
        public InputItemDataGrid(string label, string value)
        {
            this.Label = label;
            this.Value = value;
        }
        public string Label { get; set; }
        public string Value { get; set; }
    }
}
