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
        Step
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
