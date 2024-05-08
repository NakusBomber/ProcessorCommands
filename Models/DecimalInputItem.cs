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
        }

        public override string Value
        {
            get => _value;
            set
            {
                
                ClearErrors();

                if (value != string.Empty)
                {
                    if (!int.TryParse(value, out int intValue))
                        SetError("Буквы не допускаются");

                    if (intValue < 0 || intValue > 255)
                        SetError("Только байтовые значения (0 - 255)");
                }
               
                _value = value;
                OnPropertyChanged();
            }
        }
    }
}
