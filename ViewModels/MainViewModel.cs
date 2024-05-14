﻿using ProcessorCommands.Commands;
using ProcessorCommands.Helpers;
using ProcessorCommands.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProcessorCommands.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

		public MainViewModel()
		{
			Status = ProgramStatus.Nothing;
			StartCommand = new StartCommand(this);
			StopCommand = new StopCommand(this);
			StepCommand = new StepCommand(this);
			RefreshCommand = new RefreshCommand(this);
			ChangeLanguageCommand = new ChangeLanguageCommand();

			CommandRegister = new StandartInputItem();
            AldFirstRegister = new DecimalInputItem();
            AldSecondRegister = new StandartInputItem();
            ResultRegister = new StandartInputItem();
            CounterAddress = new HexadecimalInputItem();

			DataRegisters = new ObservableCollection<InputItem>();
            for (int i = 0; i < 8; i++)
            {
				DataRegisters.Add(new DecimalInputItem($"{i + 1}"));
            }
            BaseRegisters = new ObservableCollection<InputItem>();
            for (int i = 0; i < 4; i++)
            {
                BaseRegisters.Add(new HexadecimalInputItem($"{i + 1}"));
            }
            IndexRegisters = new ObservableCollection<InputItem>();
            for (int i = 0; i < 2; i++)
            {
                IndexRegisters.Add(new DecimalInputItem($"{i + 1}"));
            }

        }

		private ProgramStatus _status;
		public ProgramStatus Status
		{
			get => _status;
			set
			{
				_status = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(StatusDescription));
                OnPropertyChanged(nameof(IsBlockInput));
			}
		}

		private InputItem _commandRegister;
		public InputItem CommandRegister
		{
			get { return _commandRegister; }
			set
			{
				_commandRegister = value;
				OnPropertyChanged();
			}
		}

		private InputItem _aldFirstRegister;
		public InputItem AldFirstRegister
        {
			get { return _aldFirstRegister; }
			set
			{
				_aldFirstRegister = value;
				OnPropertyChanged();
			}
		}

        private InputItem _aldSecondRegister;
        public InputItem AldSecondRegister
        {
            get { return _aldSecondRegister; }
            set
            {
                _aldSecondRegister = value;
                OnPropertyChanged();
            }
        }

        private InputItem _resultRegister;
        public InputItem ResultRegister
        {
            get { return _resultRegister; }
            set
            {
                _resultRegister = value;
                OnPropertyChanged();
            }
        }
        
        private InputItem _counterAddress;
        public InputItem CounterAddress
        {
            get { return _counterAddress; }
            set
            {
                _counterAddress = value;
                OnPropertyChanged();
            }
        }

        #region Commands
        public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
		public ICommand StepCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand ChangeLanguageCommand { get; private set; }

        #endregion

        public string StatusDescription => Status.GetDescription();
        public bool IsBlockInput => (Status != ProgramStatus.Nothing && Status != ProgramStatus.Stop);
        public bool HasError =>  isErrorVM();

        private bool isErrorVM()
        {
            return DataRegisters.Any(item => item.HasErrors)
                || BaseRegisters.Any(e => e.HasErrors)
                || IndexRegisters.Any(e => e.HasErrors)
                || CommandRegister.HasErrors
                || AldFirstRegister.HasErrors
                || AldSecondRegister.HasErrors
                || ResultRegister.HasErrors
                || CounterAddress.HasErrors;
        }

        private ObservableCollection<InputItem> _dataRegisters;
        public ObservableCollection<InputItem> DataRegisters
        {
			get => _dataRegisters;
			private set
			{
				_dataRegisters = value;
			}
		}

        private ObservableCollection<InputItem> _baseRegisters;
        public ObservableCollection<InputItem> BaseRegisters
        {
            get => _baseRegisters;
            private set
            {
                _baseRegisters = value;
            }
        }

        private ObservableCollection<InputItem> _indexRegisters;
        public ObservableCollection<InputItem> IndexRegisters
        {
            get => _indexRegisters;
            private set
            {
                _indexRegisters = value;
            }
        }
    }
}
