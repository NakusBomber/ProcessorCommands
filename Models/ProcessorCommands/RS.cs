using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models.ProcessorCommands
{
    public class RS : TwoByteProcessorCommand
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

        protected override Task ArithmeticAlgorithm()
        {
            throw new NotImplementedException();
        }

        protected override Task DeliveryAlgorithm()
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteDelivery()
        {
            throw new NotImplementedException();
        }

        protected override Task SampleOperand(int numberOperand)
        {
            throw new NotImplementedException();
        }

        protected override Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
