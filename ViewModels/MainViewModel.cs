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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProcessorCommands.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Intel8080Model processor; 
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
            LoadSetUpCommand = new LoadSetUpCommand(this, StopCommand, RefreshCommand);
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
        }

        public async Task Start(CancellationToken token)
        {
            do
            {
                await MakeStep(token);
                await Delay();
                token.ThrowIfCancellationRequested();
            } while (Status != ProgramStatus.Finish);
        }
        
        public async Task MakeStep(CancellationToken token)
        {
            switch (Step)
            {
                case -1:
                    Status = ProgramStatus.Start;

                    Step = 0;
                    Status = ProgramStatus.SampleCommand;
                    token.ThrowIfCancellationRequested();
                    break;
                case 0:
                    Status = ProgramStatus.DecryptCommand;
                    await Delay();
                    token.ThrowIfCancellationRequested();
                    await SampleByteCommand(0, token);
                    Command = processor.GetCommand(WordRegister.Value);
                    break;
                default:
                    switch (Command)
                    {
                        case ECommands.R:
                            await R(token);
                            break;
                        case ECommands.S:
                            await S(token);
                            break;
                        case ECommands.X:
                            await X(token);
                            break;
                        case ECommands.I:
                            await I(token);
                            break;
                        case ECommands.RR:
                            await RR(token);
                            break;
                        case ECommands.RS:
                            await RS(token);
                            break;
                        case ECommands.RX:
                            await RX(token);
                            break;
                        case ECommands.SX:
                            await SX(token);
                            break;
                        case ECommands.RSX:
                            await RSX(token);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
            }

            token.ThrowIfCancellationRequested();
        }
        private async Task R(CancellationToken token) { }
        private async Task S(CancellationToken token) { }
        private async Task X(CancellationToken token) { }
        private async Task I(CancellationToken token) { }
        private async Task RR(CancellationToken token)
        {
            MaxStep = 6;
            switch (Step)
            {
                case 1:
                    await SampleByteCommand(1, token);
                    break;
                case 2:
                    await SampleOperand(0, token);
                    break;
                case 3:
                    await SampleOperand(1, token);
                    break;
                case 4:
                    await Execute(token);
                    break;
                case 5:
                    await Save(token);
                    await Delay(400);
                    Finish();
                    break;
                default:
                    break;
            }
            
            
        }
        private async Task RX(CancellationToken token) { }
        private async Task SX(CancellationToken token) { }
        private async Task RS(CancellationToken token) { }
        private async Task RSX(CancellationToken token) { }


        private async Task Delay(int millis = 1500)
        {
            await Task.Delay(millis);
        }

        private async Task SampleByteCommand(int numberByte, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            switch (numberByte)
            {
                case 0:
                    Status = ProgramStatus.Sample1Byte;
                    break;
                case 1:
                    Status = ProgramStatus.Sample2Byte;
                    break;
                case 2:
                    Status = ProgramStatus.Sample3Byte;
                    break;
                default:
                    return;
            }

            if(numberByte == 0)
            {
                await CounterAddress.Animation();
                await AddressRegister.Animation();
                AddressRegister.Value = CounterAddress.Value;
            }

            token.ThrowIfCancellationRequested();

            var value = Convert.ToInt32(AddressRegister.Value, 16);
            await RAM[value+numberByte].Animation();
            await WordRegister.Animation();

            token.ThrowIfCancellationRequested();
            WordRegister.Value = processor.ReadRAM(value+numberByte, RAM);
            await WordRegister.Animation();
            await CommandRegister.Animation();

            if (numberByte == 0)
                CommandRegister.Value = string.Empty;

            CommandRegister.Value = processor.AddByteCommand(CommandRegister.Value, WordRegister.Value);

            Step++;
        }
        private async Task SampleOperand(int numberOperand, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            switch (numberOperand)
            {
                case 0:
                    Status = ProgramStatus.Sample1Operand;
                    break;
                case 1:
                    Status = ProgramStatus.Sample2Operand;
                    break;
                default:
                    return;
            }

            var register = processor.GetIndexDataRegister(CommandRegister.Value.Split()[1], numberOperand == 0);
            await DataRegisters[register].Animation();

            token.ThrowIfCancellationRequested();

            if (numberOperand == 0)
            {
                await AluFirstRegister.Animation();
                AluFirstRegister.Value = DataRegisters[register].Value;
            }
            else
            {
                await AluSecondRegister.Animation();
                AluSecondRegister.Value = DataRegisters[register].Value;
            }
            Step++;
        }
        private async Task Execute(CancellationToken token)
        {
            Status = ProgramStatus.ExecutionCommand;

            token.ThrowIfCancellationRequested();

            await AluFirstRegister.Animation();
            await ResultRegister.Animation();
            ResultRegister.Value = AluFirstRegister.Value;

            token.ThrowIfCancellationRequested();

            await AluSecondRegister.Animation();
            await ResultRegister.Animation();
            ResultRegister.Value = processor.Accumulate(AluFirstRegister.Value, AluSecondRegister.Value);
            Step++;
        }
        private async Task Save(CancellationToken token)
        {
            Status = ProgramStatus.SaveResult;

            token.ThrowIfCancellationRequested();

            var firstByte = CommandRegister.Value.Split()[0];
            var secondByte = CommandRegister.Value.Split()[1];
            var isSaveFirstPlace = processor.isFirstPlaceSaveResult(firstByte);
            var place = processor.GetIndexDataRegister(secondByte, isSaveFirstPlace);
            await ResultRegister.Animation();
            await DataRegisters[place].Animation();
            DataRegisters[place].Value = ResultRegister.Value;
            Step++;
        }

        private void Finish()
        {
            Status = ProgramStatus.Finish;
            Command = ECommands.Unspecified;
            Step = -1;
            MaxStep = 999;
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
