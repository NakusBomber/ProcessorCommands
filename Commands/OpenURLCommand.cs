using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Commands
{
    public class OpenURLCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            var url = parameter as string;

            if (url == null || url.Length == 0)
                return;

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}
