using ProcessorCommands.Models;
using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Commands
{
    public class StopCommand : CommandBase
    {
        MainViewModel _vm;

        public StopCommand(MainViewModel vm)
        {
            _vm = vm;
        }

        public override void Execute(object parameter)
        {
            _vm.Status = ProgramStatus.Stop;
        }

        public override bool CanExecute(object parameter)
        {
            return _vm.Status != ProgramStatus.Stop;
        }
    }
}
