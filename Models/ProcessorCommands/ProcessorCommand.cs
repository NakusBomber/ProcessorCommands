using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessorCommands.Models.ProcessorCommands
{
    public abstract class ProcessorCommand
    {
        public ProcessorCommand(MainViewModel vm)
        {
            _vm = vm;
        }
        private CancellationToken _token = new CancellationToken();
        public CancellationToken Token
        {
            get => _token;
            set
            {
                _token = value;
            }
        }

        protected MainViewModel _vm;

        private int _timeMillisDelay = 1500;
        public int TimeMillisDelay
        {
            get => _timeMillisDelay;
            set
            {
                _timeMillisDelay = value;
            }
        }
        private EFlagActivate IsNeedFlagActivate()
        {
            var intValue = Convert.ToInt32(_vm.AluFirstRegister.Value) + Convert.ToInt32(_vm.AluSecondRegister.Value);
            if (intValue < -255 || intValue > 255)
                return EFlagActivate.Overflow;

            if (intValue < 0)
                return EFlagActivate.Negative;

            if(intValue == 0)
                return EFlagActivate.Zero;

            return EFlagActivate.Positive;
        }
        public async Task Delay(int? millis = null)
        {
            await Task.Delay(millis ?? TimeMillisDelay);
        }
        protected async Task SampleByteCommand(int numberByte)
        {
            Token.ThrowIfCancellationRequested();
            switch (numberByte)
            {
                case 0:
                    _vm.Status = ProgramStatus.Sample1Byte;
                    break;
                case 1:
                    _vm.Status = ProgramStatus.Sample2Byte;
                    break;
                case 2:
                    _vm.Status = ProgramStatus.Sample3Byte;
                    break;
                default:
                    return;
            }

            if (numberByte == 0)
            {
                await _vm.CounterAddress.Animation();
                await _vm.AddressRegister.Animation();
                _vm.AddressRegister.Value = _vm.CounterAddress.Value;
            }

            Token.ThrowIfCancellationRequested();

            var value = Convert.ToInt32(_vm.AddressRegister.Value, 16);
            await _vm.RAM[value + numberByte].Animation();
            await _vm.WordRegister.Animation();

            Token.ThrowIfCancellationRequested();
            _vm.WordRegister.Value = _vm.processor.ReadRAM(value + numberByte, _vm.RAM);
            await _vm.WordRegister.Animation();
            await _vm.CommandRegister.Animation();

            if (numberByte == 0)
                _vm.CommandRegister.Value = string.Empty;

            _vm.CommandRegister.Value = _vm.processor.AddByteCommand(_vm.CommandRegister.Value, _vm.WordRegister.Value);

            _vm.Step++;
        }
        protected async Task ExecuteArithmetic()
        {
            _vm.Status = ProgramStatus.ExecutionCommand;

            Token.ThrowIfCancellationRequested();

            await _vm.AluFirstRegister.Animation();
            await _vm.ResultRegister.Animation();
            _vm.ResultRegister.Value = _vm.AluFirstRegister.Value;

            Token.ThrowIfCancellationRequested();

            await _vm.AluSecondRegister.Animation();
            await _vm.ResultRegister.Animation();
            _vm.ResultRegister.Value = _vm.processor.Accumulate(_vm.AluFirstRegister.Value, _vm.AluSecondRegister.Value);

            if (Type == ETypeCommand.Arithmetic)
                await EnableFlags();

            _vm.Step++;
        }
        
        protected abstract Task SampleOperand(int numberOperand);
        protected abstract Task Save();
        
        private async Task EnableFlags()
        {
            int i = 0;
            switch (IsNeedFlagActivate())
            {
                case EFlagActivate.Negative:
                    break;
                case EFlagActivate.Zero:
                    i = 1;
                    break;
                case EFlagActivate.Positive:
                    i = 2;
                    break;
                case EFlagActivate.Overflow: 
                    i = 3;
                    break;
                default:
                    return;
            }

            Token.ThrowIfCancellationRequested();

            await _vm.FlagRegisters[i].Animation();
            _vm.FlagRegisters[i].Value = "True";
            
        }
        
        protected void Finish()
        {
            _vm.Status = ProgramStatus.Finish;
            _vm.Command = ECommands.Unspecified;
            _vm.Step = -1;
            _vm.MaxStep = 999;
        }
        
        public abstract List<string> GetErrorsValues();
        public abstract int MaxStep { get; }

        private ETypeCommand _type = ETypeCommand.Arithmetic;

        public ETypeCommand Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public abstract ECommands Command { get; }

        private bool isStartStep() => _vm.Step < 1;
        protected abstract Task MakeStepAfterStart();

        public async Task MakeStep()
        {
            if(!isStartStep())
            {
                _vm.MaxStep = MaxStep;
                await MakeStepAfterStart();
                return;
            }

            switch (_vm.Step)
            {
                case -1:
                    _vm.Status = ProgramStatus.Start;

                    _vm.Step = 0;
                    _vm.Status = ProgramStatus.SampleCommand;
                    Token.ThrowIfCancellationRequested();
                    break;
                case 0:
                    _vm.Status = ProgramStatus.DecryptCommand;
                    await Delay();
                    Token.ThrowIfCancellationRequested();
                    await SampleByteCommand(0);
                    this.Type = _vm.processor.GetTypeCommand(_vm.WordRegister.Value);
                    _vm.Command = _vm.processor.GetCommand(_vm.WordRegister.Value);
                    break;
                default:
                    break;
            } 
            
            Token.ThrowIfCancellationRequested();
        }
    }
}
