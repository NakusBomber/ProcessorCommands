using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models.ProcessorCommands
{
    public class I : ProcessorCommand
    {
        public I(MainViewModel vm) : base(vm)
        {
        }

        public override int MaxStep => 5;

        public override ECommands Command => ECommands.I;

        public override List<string> GetErrorsValues()
        {
            return new List<string>();
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

        public override async Task MakeStep()
        {
            if (_vm.Step < 1)
            {
                await base.MakeStep();
                return;
            }

            _vm.MaxStep = MaxStep;

            switch (_vm.Step)
            {
                case 1:
                    await SampleByteCommand(1);

                    if (Type == ETypeCommand.Arithmetic)
                        _vm.Step = 2;
                    else
                        _vm.Step = 3;

                    break;
                case 2:
                    await SampleOperand();
                    break;
                case 3:
                    if (Type == ETypeCommand.Arithmetic)
                    {
                        await ExecuteArithmetic();
                    }
                    if (Type == ETypeCommand.Delivery)
                    {
                        await ExecuteDelivery();
                    }
                    if(Type == ETypeCommand.UnconditionalTransfer)
                    {
                        await ExecuteUnconditional();
                    }
                    var conditionList = new List<ETypeCommand>
                    {
                        ETypeCommand.ConditionalNegative,
                        ETypeCommand.ConditionalZero,
                        ETypeCommand.ConditionalPositive,
                        ETypeCommand.ConditionalOverflow
                    };
                    if(conditionList.Contains(Type))
                    {
                        await ExecuteConditional();
                    }
                    break;
                case 4:

                    if (Type == ETypeCommand.Arithmetic)
                        await Save();

                    await Delay(400);
                    Finish();
                    break;
                default:
                    break;
            }
        }
    }
}
