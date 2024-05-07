﻿using System;
using System.Collections.Generic;
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
        Stop
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