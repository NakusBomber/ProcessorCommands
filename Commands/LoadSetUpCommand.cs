using ProcessorCommands.Models;
using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Commands
{
    public class LoadSetUpCommand : CommandBase
    {
        private MainViewModel _vm;
        private StopCommand _stopCommand;
        private RefreshCommand _refreshCommand;
        public LoadSetUpCommand(MainViewModel vm, StopCommand stopCommand, RefreshCommand refreshCommand)
        {
            _vm = vm;
            _stopCommand = stopCommand;
            _refreshCommand = refreshCommand;
        }
        
        public override void Execute(object parameter)
        {
            if (_vm.StopCommand.CanExecute(parameter))
            {
                _vm.StopCommand.Execute(parameter);
            }
            if(_vm.RefreshCommand.CanExecute(parameter))
            {
                _vm.RefreshCommand.Execute(parameter);
            }

            var param = Convert.ToInt32((parameter as string));
            var command = (ECommands)param;

            switch (command)
            {
                case ECommands.RR:
                    RR();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void RR()
        {
            _vm.CounterAddress.Value = "0x2";
            _vm.DataRegisters[3].Value = "43";
            _vm.DataRegisters[5].Value = "26";
            _vm.RAM[2].Value = "0b01000000";
            _vm.RAM[3].Value = "0b01110100";
        }
    }
}
