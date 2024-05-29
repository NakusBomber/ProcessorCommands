using ProcessorCommands.Models;
using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProcessorCommands.Commands
{
    public class StopCommand : CommandBase
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _commandExecuting;
        public CancellationToken Token { get => _cts.Token; }
        private MainViewModel _vm;
        public StopCommand(MainViewModel vm)
        {
            _vm = vm;
        }

        public override void Execute(object parameter)
        {
            _cts.Cancel();
            _vm.Status = ProgramStatus.Stop;
            _vm.Step = -1;
            _vm.MaxStep = 999;
            _vm.Command = ECommands.Unspecified;
            RaiseCanExecuteChanged();
        }

        public void NotifyCommandStarting()
        {
            _commandExecuting = true;
            _vm.IsNowExecuteCommand = true;
            if (!_cts.IsCancellationRequested)
                return;
            _cts = new CancellationTokenSource();
            RaiseCanExecuteChanged();
        }
        public void NotifyCommandFinished()
        {
            _commandExecuting = false;
            _vm.IsNowExecuteCommand = false;
            RaiseCanExecuteChanged();
        }

        public override bool CanExecute(object parameter)
        {
            
            return _vm.Status != ProgramStatus.Stop || (_commandExecuting && !_cts.IsCancellationRequested);
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
