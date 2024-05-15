using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models
{
    public abstract class StandartProcessor
    {
        public abstract int CountDataRegisters { get; }
        public abstract int CountBaseRegisters { get; }
        public abstract int CountIndexRegisters { get; }
        public abstract int SizeRAM { get; }
        public abstract int SizePage { get; }
    }
}
