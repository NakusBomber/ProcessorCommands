using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProcessorCommands.Commands
{
    public class PushZeroCommand : CommandBase
    {
        private byte countChar = 10;
        public override bool CanExecute(object parameter)
        {
            var tb = parameter as TextBox;

            if (tb == null)
                return false;

            var value = tb.Text;
            return value != null && value.Length < countChar && value.StartsWith("0b");
        }
        public override void Execute(object parameter)
        {
            var tb = parameter as TextBox;
            
            if (tb == null)
                return;

            var value = tb.Text;
            var length = value.Length;
            for (int i = 0; i < countChar - length; i++)
                value += "0";

            tb.Text = value;
            tb.CaretIndex = value.Length;
        }
    }
}
