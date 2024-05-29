using ProcessorCommands.Models;
using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessorCommands.Commands
{
    public class StepCommand : AsyncCommand<object>
    {
        private MainViewModel _vm;

        public StepCommand(MainViewModel vm, Func<CancellationToken, Task> command, StopCommand stopCommand)
            : base(async _ => { await command(stopCommand.Token); return null; }, stopCommand)
        {
            _vm = vm;
        }
        public override bool CanExecute(object parameter)
        {
            return !_vm.IsNowExecuteCommand && !_vm.HasError && base.CanExecute(parameter);
        }

    }
}
