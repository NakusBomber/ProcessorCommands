using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models
{
    public enum ECommands
    {
        R = 0,
        S = 1,
        X = 2,
        I = 3,
        RR = 4,
        RS = 5,
        RX = 6,
        SX = 7,
        RSX = 8,

        Unspecified = 999
    }
}
