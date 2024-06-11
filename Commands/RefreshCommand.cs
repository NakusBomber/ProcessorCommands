using ProcessorCommands.Models;
using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                case ProgramStatus.Finish:
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

            foreach (var item in _vm.RAM)
                item.Value = e;

            foreach (var item in _vm.FlagRegisters)
                item.Value = "False";

            _vm.CommandRegister.Value = e;
            _vm.AluFirstRegister.Value = e;
            _vm.AluSecondRegister.Value = e;
            _vm.ResultRegister.Value = e;
            _vm.CounterAddress.Value = e;
            _vm.AddressAdder.Value = e;
            _vm.AddressRegister.Value = e;
            _vm.WordRegister.Value = e;
        }
    }
}
