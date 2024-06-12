using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models.ProcessorCommands
{
    public abstract class ThreeByteProcessorCommand : ProcessorCommand
    {
        protected virtual List<ETypeCommand> SupportedTypes => new List<ETypeCommand>
        {
            ETypeCommand.Arithmetic
        };
        protected ThreeByteProcessorCommand(MainViewModel vm) : base(vm)
        {
        }
        protected abstract Task ArithmeticAlgorithm();
        protected override async Task MakeStepAfterStart()
        {
            switch (Type)
            {
                case ETypeCommand.Arithmetic:
                    await ArithmeticAlgorithm();
                    break;
                default:
                    break;
            }
        }
    }
    public abstract class TwoByteProcessorCommand : ThreeByteProcessorCommand
    {
        protected override List<ETypeCommand> SupportedTypes => new List<ETypeCommand>
        {
            ETypeCommand.Arithmetic,
            ETypeCommand.Delivery,
        };
        protected TwoByteProcessorCommand(MainViewModel vm) : base(vm)
        {
        }

        protected abstract Task DeliveryAlgorithm();
        protected abstract Task ExecuteDelivery();
        protected override async Task MakeStepAfterStart()
        {
            switch (Type)
            {
                case ETypeCommand.Arithmetic:
                    await ArithmeticAlgorithm();
                    break;
                case ETypeCommand.Delivery:
                    await DeliveryAlgorithm();
                    break;
                default:
                    break;
            }
        }
    }
    public abstract class OneByteProcessorCommand : TwoByteProcessorCommand
    {
        protected override List<ETypeCommand> SupportedTypes => new List<ETypeCommand>
        {
            ETypeCommand.Arithmetic,
            ETypeCommand.Delivery,
            ETypeCommand.UnconditionalTransfer,
            ETypeCommand.ConditionalNegative,
            ETypeCommand.ConditionalZero,
            ETypeCommand.ConditionalPositive,
            ETypeCommand.ConditionalOverflow
        };
        protected OneByteProcessorCommand(MainViewModel vm) : base(vm)
        {
        }
        private bool IsEnableFlag()
        {
            int i = ((int)Type) - 3;

            if (i < 0 || i > 3)
                return false;

            return _vm.FlagRegisters[i].Value == "True";
        }
        protected abstract Task UnconditionalAlgorithm();
        protected virtual async Task ConditionalAlgorithm()
        {
            await UnconditionalAlgorithm();
        }

        protected abstract Task ExecuteUnconditional();
        protected virtual async Task ExecuteConditional()
        {
            switch (Type)
            {
                case ETypeCommand.ConditionalNegative:
                    _vm.Status = ProgramStatus.CheckFlagNegative;
                    break;
                case ETypeCommand.ConditionalZero:
                    _vm.Status = ProgramStatus.CheckFlagZero;
                    break;
                case ETypeCommand.ConditionalPositive:
                    _vm.Status = ProgramStatus.CheckFlagPositive;
                    break;
                case ETypeCommand.ConditionalOverflow:
                    _vm.Status = ProgramStatus.CheckFlagOverflow;
                    break;
                default:
                    return;
            }

            int i = ((int)Type) - 3;

            await _vm.FlagRegisters[i].Animation();
            if (!IsEnableFlag())
            {
                Finish();
                return;
            }

            await UnconditionalAlgorithm();
        }
        protected override async Task MakeStepAfterStart()
        {
            switch(Type)
            {
                case ETypeCommand.Arithmetic:
                    await ArithmeticAlgorithm();
                    break;
                case ETypeCommand.Delivery:
                    await DeliveryAlgorithm();
                    break;
                case ETypeCommand.UnconditionalTransfer:
                    await UnconditionalAlgorithm();
                    break;
                case ETypeCommand.ConditionalNegative:
                case ETypeCommand.ConditionalZero:
                case ETypeCommand.ConditionalPositive:
                case ETypeCommand.ConditionalOverflow:
                    await ConditionalAlgorithm();
                    break;
                default:
                    break;
            }
        }
    }

}
