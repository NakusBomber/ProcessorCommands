using ProcessorCommands.Models;
using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Commands
{
    public class RefreshCommand : CommandBase
    {
        private MainViewModel _vm;

        public RefreshCommand(MainViewModel vm)
        {
            _vm = vm;
        }
        public override bool CanExecute(object parameter)
        {
            switch (_vm.Status)
            {
                case ProgramStatus.Stop:
                case ProgramStatus.Nothing:
                    return true;
                default:
                    return false;
            }
        }
        public override void Execute(object parameter)
        {
            foreach (var register in _vm.DataRegisters)
            {
                register.Value = string.Empty;
            }
        }
    }
}
