using ProcessorCommands.Models;
using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProcessorCommands.Commands
{
    // Reference: https://learn.microsoft.com/ru-ru/archive/msdn-magazine/2014/april/async-programming-patterns-for-asynchronous-mvvm-applications-commands
    public class AsyncCommand<TResult> : AsyncCommandBase
    {
        private readonly Func<CancellationToken, Task<TResult>> _command;
        private readonly StopCommand _cancelCommand;
        private NotifyTaskCompletion<TResult> _execution;

        public AsyncCommand(Func<CancellationToken, Task<TResult>> command, StopCommand stopCommand)
        {
            _command = command;
            _cancelCommand = stopCommand;
        }

        public override bool CanExecute(object parameter)
        {
            return Execution == null || Execution.IsCompleted;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _cancelCommand.NotifyCommandStarting();
            Execution = new NotifyTaskCompletion<TResult>(_command(_cancelCommand.Token));
            RaiseCanExecuteChanged();
            await Execution.TaskCompletion;
            _cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
        }

        public NotifyTaskCompletion<TResult> Execution
        {
            get { return _execution; }
            private set
            {
                _execution = value;
                OnPropertyChanged();
            }
        }
    }

}
