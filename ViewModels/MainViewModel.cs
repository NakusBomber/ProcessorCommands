using ProcessorCommands.Commands;
using ProcessorCommands.Helpers;
using ProcessorCommands.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
			StepCommand = new StepCommand(this);
			RefreshCommand = new RefreshCommand(this);
			ChangeLanguageCommand = new ChangeLanguageCommand();

			CommandRegister = new HexadecimalInputItem("", "");
			DataRegisters = new ObservableCollection<InputItem>();
            for (int i = 0; i < 8; i++)
            {
				DataRegisters.Add(new DecimalInputItem($"{i+1}", ""));
            }

        }

		private ProgramStatus _status;
		public ProgramStatus Status
		{
			get => _status;
			set
			{
				_status = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(StatusDescription));
                OnPropertyChanged(nameof(IsBlockInput));
			}
		}

		private HexadecimalInputItem _commandRegister;

		public HexadecimalInputItem CommandRegister
		{
			get { return _commandRegister; }
			set
			{
				_commandRegister = value;
				OnPropertyChanged();
			}
		}


		#region Commands
		public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
		public ICommand StepCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand ChangeLanguageCommand { get; private set; }

        #endregion

        public string StatusDescription => Status.GetDescription();
        public bool IsBlockInput => (Status != ProgramStatus.Nothing && Status != ProgramStatus.Stop);
        public bool HasError => DataRegisters.Any(item => item.HasErrors) || CommandRegister.HasErrors;

        private ObservableCollection<InputItem> _dataRegisters;
        public ObservableCollection<InputItem> DataRegisters
        {
			get => _dataRegisters;
			private set
			{
				_dataRegisters = value;
			}
		}

    }
}
