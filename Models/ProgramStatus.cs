using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ProcessorCommands.Models
{
    public enum ProgramStatus
    {
        Nothing,
        Start,
        Stop,
        Step,
        SampleCommand,
        Sample1Byte,
        Sample2Byte,
        Sample3Byte,
        Sample1Operand,
        Sample2Operand,
        DecryptCommand,
        ExecutionCommand,
        SaveResult,
        WithoutConditionTransfer,
        CheckFlagNegative,
        CheckFlagZero,
        CheckFlagPositive,
        CheckFlagOverflow,
        Finish
    }

    public static class ProgramStatusExtensions
    {

        public static string GetDescription(this ProgramStatus status)
        {
            var resourceManager = new ResourceManager("ProcessorCommands.Resources.ProgramStatus", typeof(ProgramStatus).Assembly);
            return resourceManager.GetString(status.ToString());
        }
    }
}
