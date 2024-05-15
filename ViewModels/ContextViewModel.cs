using ProcessorCommands.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProcessorCommands.ViewModels
{
    public class ContextViewModel : ViewModelBase
    {
        public ContextViewModel()
        {
            PushZeroCommand = new PushZeroCommand();
        }
        public ICommand PushZeroCommand { get; private set; }
    }
}
