using ProcessorCommands.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Commands
{
    public class ChangeLanguageCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            var lang = parameter as string;
            LanguageManager.Change(lang);
        }

        public override bool CanExecute(object parameter)
        {
            var lang = parameter as string;
            return LanguageManager.CurrentLanguage != lang;
        }
    }
}
