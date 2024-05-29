using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models
{
    public enum ETypeCommand
    {
        Arithmetic,
        Delivery,
        UnconditionalTransfer,
        ConditionalTransfer,

        Unspecified = 999
    }
}
