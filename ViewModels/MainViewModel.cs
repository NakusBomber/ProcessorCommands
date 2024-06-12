using ProcessorCommands.Commands;
using ProcessorCommands.Helpers;
using ProcessorCommands.Models;
using ProcessorCommands.Models.ProcessorCommands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProcessorCommands.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public Intel8080Model processor; 
        private ProcessorCommand processorCommand = null;
        public MainViewModel()
		{
            processor = new Intel8080Model();
			Status = ProgramStatus.Nothing;
            Command = ECommands.Unspecified;
            Step = -1;
            MaxStep = 999;
			CreateCommands();
            CreateRegisters();
        }

        private void CreateCommands()
        {
            StopCommand = new StopCommand(this);
            StartCommand = new StartCommand(this, Start, StopCommand);
            StepCommand = new StepCommand(this, MakeStep, StopCommand);
            RefreshCommand = new RefreshCommand(this);
            ChangeLanguageCommand = new ChangeLanguageCommand();
            LoadSetUpCommand = new LoadSetUpCommand(this);
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
                DataRegisters.Add(new DecimalInputItem($"{i}"));
            }
            BaseRegisters = new ObservableCollection<InputItem>();
            for (int i = 0; i < processor.CountBaseRegisters; i++)
            {
                BaseRegisters.Add(new HexadecimalInputItem($"{i}"));
            }
            IndexRegisters = new ObservableCollection<InputItem>();
            for (int i = 0; i < processor.CountIndexRegisters; i++)
            {
                IndexRegisters.Add(new DecimalInputItem($"{i}"));
            }
            RAM = new ObservableCollection<InputItem>();
            for (int i = 0; i < processor.SizeRAM; i++)
            {
                RAM.Add(new BinaryDecimalInputItem($"{i:X2}"));
            }

            FlagRegisters = new ObservableCollection<InputItem>();
            var flagLabels = new List<string>
            {
                Resources.General.Negative,
                Resources.General.Zero,
                Resources.General.Positive,
                Resources.General.Overflow,
            };
            for(int i = 0; i < 4; i++)
            {
                FlagRegisters.Add(new BoolInputItem(flagLabels[i], "False"));
            }
        }

        private bool CanStartExecuteCommand()
        {
            
            processorCommand = ProcessorCommandCreator.Create(this);

            if (processorCommand == null)
                return false;

            var errors = processorCommand.GetErrorsValues();
            if (errors.Count > 0)
            {
                var errStr = string.Empty;
                foreach (var error in errors)
                {
                    errStr += error + "\n";
                }
                MessageBox.Show(errStr);
                return false;
            }

            return true;
        }
        public async Task Start(CancellationToken token)
        {
            if (!CanStartExecuteCommand())
                return;

            do
            {
                processorCommand.Token = token;
                await processorCommand.MakeStep();
                await processorCommand.Delay();
                processorCommand.Token.ThrowIfCancellationRequested();
            } while (Status != ProgramStatus.Finish);
        }
        public async Task MakeStep(CancellationToken token)
        {
            if (!CanStartExecuteCommand())
                return;

            processorCommand.Token = token;
            await processorCommand.MakeStep();
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

        private ECommands _command;

        public ECommands Command
        {
            get { return _command; }
            set
            {
                _command = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CommandDescription));
                OnPropertyChanged(nameof(StepVisibility));
            }
        }

        public string CommandDescription => Command.ToString();

        private int _step;
        public int Step
        {
            get { return _step; }
            set
            {
                _step = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StepVisibility));
            }
        }


        private int _maxStep;
        public int MaxStep
        {
            get { return _maxStep; }
            set
            {
                _maxStep = value;
                OnPropertyChanged();
            }
        }

        private bool _isNowExecuteCommand;

        public bool IsNowExecuteCommand
        {
            get { return _isNowExecuteCommand; }
            set
            {
                _isNowExecuteCommand = value;
                OnPropertyChanged();
            }
        }


        #region Registers

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

        #endregion

        #region Commands
        public StartCommand StartCommand { get; private set; }
        public StopCommand StopCommand { get; private set; }
		public StepCommand StepCommand { get; private set; }
        public RefreshCommand RefreshCommand { get; private set; }
        public ICommand ChangeLanguageCommand { get; private set; }
        public ICommand LoadSetUpCommand { get; private set; }

        #endregion

        public Visibility StepVisibility => (Step != -1 && Command != ECommands.Unspecified) ? Visibility.Visible : Visibility.Hidden;
        public string StatusDescription => Status.GetDescription();
        public bool IsBlockInput => (Status != ProgramStatus.Nothing && 
                                        Status != ProgramStatus.Stop &&
                                        Status != ProgramStatus.Finish);
        public bool HasError => isErrorVM();

        private bool isErrorVM()
        {
            return DataRegisters.Any(item => item.HasErrors)
                || BaseRegisters.Any(e => e.HasErrors)
                || IndexRegisters.Any(e => e.HasErrors)
                || RAM.Any(e => e.HasErrors)
                || FlagRegisters.Any(e => e.HasErrors)
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

        private ObservableCollection<InputItem> _flagRegisters;
        public ObservableCollection<InputItem> FlagRegisters
        {
            get { return _flagRegisters; }
            private set
            {
               _flagRegisters = value;
            }
        }

    }
}
