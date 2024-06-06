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
        protected virtual async Task SampleOperand(int numberOperand)
        {
            Token.ThrowIfCancellationRequested();

            switch (numberOperand)
            {
                case 0:
                    _vm.Status = ProgramStatus.Sample1Operand;
                    break;
                case 1:
                    _vm.Status = ProgramStatus.Sample2Operand;
                    break;
                default:
                    return;
            }

            var register = _vm.processor.GetIndexDataRegister(_vm.CommandRegister.Value.Split()[1], numberOperand == 0);
            await _vm.DataRegisters[register].Animation();

            Token.ThrowIfCancellationRequested();

            if (numberOperand == 0)
            {
                await _vm.AluFirstRegister.Animation();
                _vm.AluFirstRegister.Value = _vm.DataRegisters[register].Value;
            }
            else
            {
                await _vm.AluSecondRegister.Animation();
                _vm.AluSecondRegister.Value = _vm.DataRegisters[register].Value;
            }
            _vm.Step++;
        }
        protected virtual async Task ExecuteArithmetic()
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
 
            _vm.Step++;
        }
        protected virtual async Task ExecuteDelivery()
        {
            _vm.Status = ProgramStatus.ExecutionCommand;

            Token.ThrowIfCancellationRequested();

            var firstByte = _vm.CommandRegister.Value.Split()[0];
            var secondByte = _vm.CommandRegister.Value.Split()[1];
            var isFirstSavePlace = _vm.processor.isFirstPlaceSaveResult(firstByte);

            var firstIndex = _vm.processor.GetIndexDataRegister(secondByte, true);
            var secondIndex = _vm.processor.GetIndexDataRegister(secondByte, false);

            var saveIndex = isFirstSavePlace ? firstIndex : secondIndex;
            var dataIndex = isFirstSavePlace ? secondIndex : firstIndex;

            await _vm.DataRegisters[dataIndex].Animation();
            await _vm.DataRegisters[saveIndex].Animation();

            Token.ThrowIfCancellationRequested();
            _vm.DataRegisters[saveIndex].Value = _vm.DataRegisters[dataIndex].Value;

            _vm.Step++;
        }
        protected virtual async Task Save()
        {
            _vm.Status = ProgramStatus.SaveResult;

            Token.ThrowIfCancellationRequested();

            var firstByte = _vm.CommandRegister.Value.Split()[0];
            var secondByte = _vm.CommandRegister.Value.Split()[1];
            var isSaveFirstPlace = _vm.processor.isFirstPlaceSaveResult(firstByte);
            var place = _vm.processor.GetIndexDataRegister(secondByte, isSaveFirstPlace);
            await _vm.ResultRegister.Animation();
            await _vm.DataRegisters[place].Animation();
            _vm.DataRegisters[place].Value = _vm.ResultRegister.Value;
            
            _vm.Step++;
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

        public virtual async Task MakeStep()
        {
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
