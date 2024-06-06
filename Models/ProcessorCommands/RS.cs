using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models.ProcessorCommands
{
    public class RS : ProcessorCommand
    {
        public RS(MainViewModel vm) : base(vm)
        {
        }

        public override int MaxStep => throw new NotImplementedException();

        public override ECommands Command => throw new NotImplementedException();

        public override List<string> GetErrorsValues()
        {
            throw new NotImplementedException();
        }
    }
}
