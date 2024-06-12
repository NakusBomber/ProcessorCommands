using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models.ProcessorCommands
{
    public class R : OneByteProcessorCommand
    {
        public R(MainViewModel vm) : base(vm)
        {
        }
        public override ECommands Command => ECommands.R;
        public override int MaxStep
        {
            get
            {
                if(Type == ETypeCommand.Arithmetic)
                {
                    return 5;
                }
                return 4;
            }
        }

        
        public override List<string> GetErrorsValues()
        {
            var errors = new List<string>();

            var intAddress = Convert.ToInt32(_vm.CounterAddress.Value, 16);
            var firstByte = _vm.RAM[intAddress].Value;
            var typeCommand = _vm.processor.GetTypeCommand(firstByte);
            var isFirstSavePlace = _vm.processor.isFirstPlaceSaveResult(firstByte);

            if (!SupportedTypes.Contains(typeCommand))
            {
                errors.Add("This type of command does not exist");
                return errors;
            }

            if (intAddress >= 255 || _vm.RAM[intAddress + 1].Value == string.Empty)
            {
                errors.Add("Not found second byte command");
                return errors;
            }

            var secondByte = _vm.RAM[intAddress + 1].Value;
            var value1 = _vm.AluFirstRegister.Value;
            var value2 = _vm.DataRegisters[_vm.processor.GetIndexDataRegister(secondByte, true)].Value;

            if (typeCommand != ETypeCommand.Delivery ||
                (typeCommand == ETypeCommand.Delivery && !isFirstSavePlace))
            {
                if (value1 == string.Empty)
                {
                    errors.Add("Empty alu first register value");
                }
            }

            if (typeCommand != ETypeCommand.Delivery ||
                (typeCommand == ETypeCommand.Delivery && isFirstSavePlace))
            {
                if (value2 == string.Empty)
                {
                    errors.Add("Empty second value");
                }
            }

            return errors;
        }

        protected override async Task ArithmeticAlgorithm()
        {
            switch (_vm.Step)
            {
                case 1:
                    await SampleByteCommand(1);
                    break;
                case 2:
                    await SampleOperand();
                    break;
                case 3:
                    await ExecuteDelivery();
                    break;
                case 4:
                    await Save();
                    await Delay(400);
                    Finish();
                    break;
                default:
                    break;
            }
        }

        protected override async Task DeliveryAlgorithm()
        {
            switch (_vm.Step)
            {
                case 1:
                    await SampleByteCommand(1);
                    break;
                case 2:
                    await ExecuteDelivery();
                    break;
                case 3:
                    await Delay(400);
                    Finish();
                    break;
                default:
                    break;
            }
        }
        protected override async Task UnconditionalAlgorithm()
        {
            switch (_vm.Step)
            {
                case 1:
                    await SampleByteCommand(1);
                    break;
                case 2:
                    await ExecuteConditional();
                    break;
                case 3:
                    await Delay(400);
                    Finish();
                    break;
                default:
                    break;
            }
        }

        protected override async Task ExecuteUnconditional()
        {
            if (Type == ETypeCommand.UnconditionalTransfer)
            {
                _vm.Status = ProgramStatus.WithoutConditionTransfer;
            }

            Token.ThrowIfCancellationRequested();

            var place = _vm.processor.GetIndexDataRegister(_vm.CommandRegister.Value.Split()[1], true);
            var data = Convert.ToInt32(_vm.DataRegisters[place].Value);

            if (data < 0)
                data = 0;

            var dataString = $"0x{data:X2}";
            await _vm.DataRegisters[place].Animation();

            await _vm.CounterAddress.Animation();
            _vm.CounterAddress.Value = dataString;

            if (Type != ETypeCommand.UnconditionalTransfer)
            {
                foreach (var item in _vm.FlagRegisters)
                {
                    item.Value = "False";
                }
            }

            _vm.Step++;
        }
        protected override async Task Save()
        {
            _vm.Status = ProgramStatus.SaveResult;

            Token.ThrowIfCancellationRequested();

            var firstByte = _vm.CommandRegister.Value.Split()[0];
            var secondByte = _vm.CommandRegister.Value.Split()[1];
            var isSaveFirstPlace = _vm.processor.isFirstPlaceSaveResult(firstByte);
            
            if(isSaveFirstPlace)
            {
                await _vm.ResultRegister.Animation();
                await _vm.AluFirstRegister.Animation();
                _vm.AluFirstRegister.Value = _vm.ResultRegister.Value;
            }
            else
            {
                var place = _vm.processor.GetIndexDataRegister(secondByte, isSaveFirstPlace);
                await _vm.ResultRegister.Animation();
                await _vm.DataRegisters[place].Animation();
                _vm.DataRegisters[place].Value = _vm.ResultRegister.Value;
            }

            _vm.Step++;
        }

        protected override async Task ExecuteDelivery()
        {
            _vm.Status = ProgramStatus.ExecutionCommand;

            Token.ThrowIfCancellationRequested();

            var firstByte = _vm.CommandRegister.Value.Split()[0];
            var secondByte = _vm.CommandRegister.Value.Split()[1];
            var isFirstSavePlace = _vm.processor.isFirstPlaceSaveResult(firstByte);

            var index = _vm.processor.GetIndexDataRegister(secondByte, true);

            if (isFirstSavePlace)
            {
                await _vm.DataRegisters[index].Animation();
                await _vm.AluFirstRegister.Animation();

                Token.ThrowIfCancellationRequested();

                _vm.AluFirstRegister.Value = _vm.DataRegisters[index].Value;
            }
            else
            {
                await _vm.AluFirstRegister.Animation();
                await _vm.DataRegisters[index].Animation();

                Token.ThrowIfCancellationRequested();

                _vm.DataRegisters[index].Value = _vm.AluFirstRegister.Value;
            }

            _vm.Step++;
        }

        protected override async Task SampleOperand(int numberOperand = 0)
        {
            Token.ThrowIfCancellationRequested();

            _vm.Status = ProgramStatus.Sample2Operand;

            var register = _vm.processor.GetIndexDataRegister(_vm.CommandRegister.Value.Split()[1], true);
            await _vm.DataRegisters[register].Animation();

            await _vm.AluSecondRegister.Animation();
            _vm.AluSecondRegister.Value = _vm.DataRegisters[register].Value;

            _vm.Step++;
        }
    }
}
