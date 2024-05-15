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
        private StandartProcessor processor = new Intel8080Model(); 
		public MainViewModel()
		{
			Status = ProgramStatus.Nothing;
			CreateCommands();
            CreateRegisters();
        }

        private void CreateCommands()
        {
            StartCommand = new StartCommand(this);
            StopCommand = new StopCommand(this);
            StepCommand = new StepCommand(this);
            RefreshCommand = new RefreshCommand(this);
            ChangeLanguageCommand = new ChangeLanguageCommand();
        }

        private void CreateRegisters()
        {
            CommandRegister = new StandartInputItem();
            AluFirstRegister = new DecimalInputItem();
            AluSecondRegister = new StandartInputItem();
            ResultRegister = new StandartInputItem();
            CounterAddress = new HexadecimalInputItem();
            AddressRegister = new HexadecimalInputItem();
            WordRegister = new BinaryDecimalInputItem();
            AddressAdder = new HexadecimalInputItem();

            DataRegisters = new ObservableCollection<InputItem>();
            for (int i = 0; i < processor.CountDataRegisters; i++)
            {
                DataRegisters.Add(new DecimalInputItem($"{i + 1}"));
            }
            BaseRegisters = new ObservableCollection<InputItem>();
            for (int i = 0; i < processor.CountBaseRegisters; i++)
            {
                BaseRegisters.Add(new HexadecimalInputItem($"{i + 1}"));
            }
            IndexRegisters = new ObservableCollection<InputItem>();
            for (int i = 0; i < processor.CountIndexRegisters; i++)
            {
                IndexRegisters.Add(new DecimalInputItem($"{i + 1}"));
            }
            RAM = new ObservableCollection<InputItem>();
            for (int i = 0; i < processor.SizeRAM; i++)
            {
                RAM.Add(new BinaryDecimalInputItem($"{i:X2}"));
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

		private InputItem _commandRegister;
		public InputItem CommandRegister
		{
			get { return _commandRegister; }
			set
			{
				_commandRegister = value;
				OnPropertyChanged();
			}
		}

		private InputItem _aluFirstRegister;
		public InputItem AluFirstRegister
        {
			get { return _aluFirstRegister; }
			set
			{
				_aluFirstRegister = value;
				OnPropertyChanged();
			}
		}

        private InputItem _aluSecondRegister;
        public InputItem AluSecondRegister
        {
            get { return _aluSecondRegister; }
            set
            {
                _aluSecondRegister = value;
                OnPropertyChanged();
            }
        }

        private InputItem _resultRegister;
        public InputItem ResultRegister
        {
            get { return _resultRegister; }
            set
            {
                _resultRegister = value;
                OnPropertyChanged();
            }
        }
        
        private InputItem _counterAddress;
        public InputItem CounterAddress
        {
            get { return _counterAddress; }
            set
            {
                _counterAddress = value;
                OnPropertyChanged();
            }
        }

        private InputItem _addressRegister;
        public InputItem AddressRegister
        {
            get { return _addressRegister; }
            set
            {
                _addressRegister = value;
                OnPropertyChanged();
            }
        }

        private InputItem _wordRegister;
        public InputItem WordRegister
        {
            get { return _wordRegister; }
            set
            {
                _wordRegister = value;
                OnPropertyChanged();
            }
        }

        private InputItem _addressAdder;
        public InputItem AddressAdder
        {
            get { return _addressAdder; }
            set
            {
                _addressAdder = value;
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
        public bool HasError =>  isErrorVM();

        private bool isErrorVM()
        {
            return DataRegisters.Any(item => item.HasErrors)
                || BaseRegisters.Any(e => e.HasErrors)
                || IndexRegisters.Any(e => e.HasErrors)
                || RAM.Any(e => e.HasErrors)
                || CommandRegister.HasErrors
                || AluFirstRegister.HasErrors
                || AluSecondRegister.HasErrors
                || ResultRegister.HasErrors
                || CounterAddress.HasErrors
                || AddressRegister.HasErrors
                || AddressAdder.HasErrors
                || WordRegister.HasErrors;
        }

        private ObservableCollection<InputItem> _dataRegisters;
        public ObservableCollection<InputItem> DataRegisters
        {
			get => _dataRegisters;
			private set
			{
				_dataRegisters = value;
			}
		}

        private ObservableCollection<InputItem> _baseRegisters;
        public ObservableCollection<InputItem> BaseRegisters
        {
            get => _baseRegisters;
            private set
            {
                _baseRegisters = value;
            }
        }

        private ObservableCollection<InputItem> _indexRegisters;
        public ObservableCollection<InputItem> IndexRegisters
        {
            get => _indexRegisters;
            private set
            {
                _indexRegisters = value;
            }
        }
        
        private ObservableCollection<InputItem> _ram;
        public ObservableCollection<InputItem> RAM
        {
            get => _ram;
            private set
            {
                _ram = value;
            }
        }
    }
}
