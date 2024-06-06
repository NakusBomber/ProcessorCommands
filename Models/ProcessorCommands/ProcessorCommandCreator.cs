using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models.ProcessorCommands
{
    public static class ProcessorCommandCreator
    {
        public static ProcessorCommand Create(MainViewModel vm)
        {
            var intAddress = Convert.ToInt32(vm.CounterAddress.Value, 16);

            try
            {
                var command = vm.processor.GetCommand(vm.RAM[intAddress].Value);

                switch (command)
                {
                    case ECommands.R:
                        return new R(vm);
                    case ECommands.S:
                        return new S(vm);
                    case ECommands.X:
                        return new X(vm);
                    case ECommands.I:
                        return new I(vm);
                    case ECommands.RR:
                        return new RR(vm);
                    case ECommands.RS:
                        return new RS(vm);
                    case ECommands.RX:
                        return new RX(vm);
                    case ECommands.SX:
                        return new SX(vm);
                    case ECommands.RSX:
                        return new RSX(vm);
                    case ECommands.Unspecified:
                    default:
                        return null;
                }
            }
            catch (CommandNotFound)
            {
                return null;
            }
        }

    }
}
