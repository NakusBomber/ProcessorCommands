using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models
{
    public class CommandNotFound : Exception
    {
        public CommandNotFound() : base("Not implemented this command in processor")
        {
            
        }
    }
    public abstract class StandartProcessor
    {
        public abstract int CountDataRegisters { get; }
        public abstract int CountBaseRegisters { get; }
        public abstract int CountIndexRegisters { get; }
        public abstract int SizeRAM { get; }
        public abstract int SizePage { get; }
        
        public int GetIndexDataRegister(string secondByteCommand, bool isFirst)
        {
            if (secondByteCommand == null || secondByteCommand == string.Empty)
                throw new ArgumentException($"{nameof(secondByteCommand)} must be not null or empty");

            if(isFirst)
            {
                return Convert.ToInt32(secondByteCommand.Replace("0b", "").Substring(0, 3), 2);
            }
            else
            {
                return Convert.ToInt32(secondByteCommand.Replace("0b", "").Substring(3, 3), 2);
            }
        }
        public ETypeCommand GetTypeCommand(string firstByteCommand)
        {
            var intByte = Convert.ToInt32(firstByteCommand.Replace("0b", "").Substring(5, 3), 2);

            return (ETypeCommand)intByte;
        }

        public bool isFirstPlaceSaveResult(string firstByteCommand)
        {
            var intByte = Convert.ToInt32(firstByteCommand.Replace("0b", "").Substring(4, 1), 2);

            return intByte == 0;
        }
        public ECommands GetCommand(string firstByteCommand)
        {
            var intByte = Convert.ToInt32(firstByteCommand.Replace("0b", "").Substring(0, 4), 2);

            if (!Enum.IsDefined(typeof(ECommands), intByte))
                throw new CommandNotFound();

            return (ECommands)intByte;
        }
        public string ReadRAM(string address, ICollection<InputItem> ram)
        {
            if (ram == null)
                throw new ArgumentNullException(nameof(ram));

            if (address == string.Empty)
                throw new ArgumentException($"{nameof(address)} must be not empty");
            
            var intAddress = Convert.ToInt32(address, 16);
            return ReadRAM(intAddress, ram);
        }

        public string ReadRAM(int address, ICollection<InputItem> ram) => ram.ElementAt(address).Value;

        public string AddByteCommand(string line, string value)
        {
            if (value == string.Empty)
                throw new ArgumentException($"{nameof(value)} must be not empty");

            if (line == string.Empty)
                return value;

            return $"{line} {value}";
        }

        public string Accumulate(string value1, string value2)
        {
            int v1, v2;
            v1 = Convert.ToInt32(value1);
            v2 = Convert.ToInt32(value2);

            int v3 = v1 + v2;
            if (v3 > 255)
            {
                v3 -= 255;
            }
            else if (v3 < -255)
            {
                v3 += 255;
                v3 *= -1;
            }

            return v3.ToString();
        }
    }
}
