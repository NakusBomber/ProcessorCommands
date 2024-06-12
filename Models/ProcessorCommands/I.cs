using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models.ProcessorCommands
{
    public class I : OneByteProcessorCommand
    {
        public I(MainViewModel vm) : base(vm)
        {
        }

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

        public override ECommands Command => ECommands.I;

        public override List<string> GetErrorsValues()
        {
            var errors = new List<string>();

            var intAddress = Convert.ToInt32(_vm.CounterAddress.Value, 16);
            var firstByte = _vm.RAM[intAddress].Value;
            var typeCommand = _vm.processor.GetTypeCommand(firstByte);

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

            var value1 = _vm.AluFirstRegister.Value;

            if (typeCommand == ETypeCommand.Arithmetic)
            {
                if (value1 == string.Empty)
                {
                    errors.Add("Empty alu first register value");
                }
            }

            
            return errors;
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
                    await ExecuteArithmetic();
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

        protected override async Task ExecuteDelivery()
        {
            Token.ThrowIfCancellationRequested();

            _vm.Status = ProgramStatus.ExecutionCommand;

            var data = Convert.ToInt32(_vm.CommandRegister.Value.Split()[1].Replace("0b", ""), 2);
            await _vm.CommandRegister.Animation();

            await _vm.AluSecondRegister.Animation();
            _vm.AluSecondRegister.Value = data.ToString();

            _vm.Step++;
        }
        protected override async Task SampleOperand(int numberOperand = 0)
        {
            Token.ThrowIfCancellationRequested();

            _vm.Status = ProgramStatus.Sample2Operand;

            var data = Convert.ToInt32(_vm.CommandRegister.Value.Split()[1].Replace("0b", ""), 2);
            await _vm.CommandRegister.Animation();

            await _vm.AluSecondRegister.Animation();
            _vm.AluSecondRegister.Value = data.ToString();

            _vm.Step++;
        }
        protected override async Task ExecuteUnconditional()
        {
            if(Type == ETypeCommand.UnconditionalTransfer)
            {
                _vm.Status = ProgramStatus.WithoutConditionTransfer;
            }

            Token.ThrowIfCancellationRequested();

            var data = Convert.ToInt32(_vm.CommandRegister.Value.Split()[1].Replace("0b", ""), 2);
            var dataString = $"0x{data:X2}";
            await _vm.CommandRegister.Animation();

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
            Token.ThrowIfCancellationRequested();

            _vm.Status = ProgramStatus.SaveResult;

            await _vm.ResultRegister.Animation();
            await _vm.AluFirstRegister.Animation();

            _vm.AluFirstRegister.Value = _vm.ResultRegister.Value;

            _vm.Step++;
        }

    }
}
