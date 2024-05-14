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
            var e = string.Empty;
            foreach (var register in _vm.DataRegisters)
                register.Value = e;

            foreach (var reg in _vm.BaseRegisters)
                reg.Value = e;

            foreach (var reg in _vm.IndexRegisters)
                reg.Value = e;

            _vm.CommandRegister.Value = e;
            _vm.AldFirstRegister.Value = e;
            _vm.AldSecondRegister.Value = e;
            _vm.ResultRegister.Value = e;
        }
    }
}
