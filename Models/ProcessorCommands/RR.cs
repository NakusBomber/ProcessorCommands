using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models.ProcessorCommands
{
    public class RR : ProcessorCommand
    {
        public RR(MainViewModel vm) : base(vm)
        {
        }

        public override int MaxStep {
            get => 6;
        }

        public override ECommands Command => ECommands.RR;

        public override List<string> GetErrorsValues()
        {
            var errors = new List<string>();

            var intAddress = Convert.ToInt32(_vm.CounterAddress.Value, 16);
            var firstByte = _vm.RAM[intAddress].Value;
            var typeCommand = _vm.processor.GetTypeCommand(firstByte);
            var isFirstSavePlace = _vm.processor.isFirstPlaceSaveResult(firstByte);
            if (typeCommand != ETypeCommand.Arithmetic && typeCommand != ETypeCommand.Delivery)
            {
                errors.Add("This type of command does not exist");
                return errors;
            }

            if(intAddress >= 255 || _vm.RAM[intAddress+1].Value == string.Empty)
            {
                errors.Add("Not found second byte command");
                return errors;
            }

            var secondByte = _vm.RAM[intAddress + 1].Value;
            var value1 = _vm.DataRegisters[_vm.processor.GetIndexDataRegister(secondByte, true)].Value;
            if(typeCommand == ETypeCommand.Arithmetic || 
                (typeCommand == ETypeCommand.Delivery && !isFirstSavePlace))
            {
                if (value1 == string.Empty)
                {
                    errors.Add("Empty first value");
                }
            }
            

            var value2 = _vm.DataRegisters[_vm.processor.GetIndexDataRegister(secondByte, false)].Value;
            if (typeCommand == ETypeCommand.Arithmetic ||
                (typeCommand == ETypeCommand.Delivery && isFirstSavePlace))
            {
                if (value2 == string.Empty)
                {
                    errors.Add("Empty second value");
                }
            }
            if (typeCommand == ETypeCommand.Arithmetic && value2 != string.Empty && value1 != string.Empty)
            {
                if (Convert.ToInt32(value1) + Convert.ToInt32(value2) > 255)
                {
                    errors.Add("Overflow byte");
                }
            }
            return errors;
        }

        public override async Task MakeStep()
        {
            if(_vm.Step < 1)
            {
                await base.MakeStep();
                return;
            }

            _vm.MaxStep = MaxStep;


            switch (_vm.Step)
            {
                case 1:
                    await SampleByteCommand(1);

                    if(Type == ETypeCommand.Delivery)
                        _vm.Step = 4;

                    break;
                case 2:
                    await SampleOperand(0);
                    break;
                case 3:
                    await SampleOperand(1);
                    break;
                case 4:
                    if(Type == ETypeCommand.Arithmetic)
                    {
                        await ExecuteArithmetic();
                    }
                    else
                    {
                        await ExecuteDelivery();
                    }
                    break;
                case 5:
                    
                    if(Type == ETypeCommand.Arithmetic)
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
