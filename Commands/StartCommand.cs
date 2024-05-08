﻿using ProcessorCommands.Helpers;
using ProcessorCommands.Models;
using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProcessorCommands.Commands
{
    public class StartCommand : CommandBase
    {
        private MainViewModel _vm;
        public StartCommand(MainViewModel vm)
        {
            _vm = vm;
        }

        public override void Execute(object parameter)
        {
            _vm.Status = ProgramStatus.Start;
        }

        public override bool CanExecute(object parameter)
        {
            return _vm.Status != ProgramStatus.Start && !_vm.HasError;
        }

    }
}
