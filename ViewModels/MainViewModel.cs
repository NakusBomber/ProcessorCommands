using ProcessorCommands.Commands;
using ProcessorCommands.Helpers;
using ProcessorCommands.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProcessorCommands.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

		public MainViewModel()
		{
			Status = ProgramStatus.Nothing;
			StartCommand = new StartCommand(this);
			StopCommand = new StopCommand(this);
			ChangeLanguageCommand = new ChangeLanguageCommand();

			DataRegisters = new ObservableCollection<InputItemDataGrid>();
            for (int i = 0; i < 8; i++)
            {
				DataRegisters.Add(new InputItemDataGrid($"{i+1}", ""));
            }

        }

		private ProgramStatus _status;
		public ProgramStatus Status
		{
			get => _status;
			set
			{
				_status = value;
				StatusDescription = _status.GetDescription();
                OnPropertyChanged();
			}
		}

		private string _statusDescription;
		public string StatusDescription
		{
            get => _statusDescription;
			private set
			{
				_statusDescription = value;
				OnPropertyChanged();
			}
        }

        private ICommand _startCommand;
		public ICommand StartCommand
        {
			get => _startCommand;
			private set
			{
				_startCommand = value;
			}
		}

        private ICommand _stopCommand;
        public ICommand StopCommand
        {
            get => _stopCommand;
            private set
			{
				_stopCommand = value;
			}
        }

		private ICommand _changeLanguageCommand;
		public ICommand ChangeLanguageCommand
		{
			get => _changeLanguageCommand;
			private set
			{
				_changeLanguageCommand = value;
			}
		}


		private ObservableCollection<InputItemDataGrid> _dataRegisters;
		public ObservableCollection<InputItemDataGrid> DataRegisters
        {
			get => _dataRegisters;
			private set
			{
				_dataRegisters = value;
			}
		}

	}
}
