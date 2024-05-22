using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models
{
    public class Intel8080Model : StandartProcessor
    {
        
        public override int CountDataRegisters => 8;

        public override int CountBaseRegisters => 4;

        public override int CountIndexRegisters => 2;

        public override int SizeRAM => 256;

        public override int SizePage => 256;

    }
}
