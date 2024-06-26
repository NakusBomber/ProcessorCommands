﻿using ProcessorCommands.Helpers.Validations;
using ProcessorCommands.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessorCommands.Models
{
    public abstract class InputItem : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public InputItem(string label, string value)
        {
            this.Label = label ?? "";
            this.Value = value ?? "";
        }

        public string Label { get; set; }

        protected IValidateValue Validation { get; set; }

        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                ClearErrors();

                if(Validation != null)
                {
                    var errors = Validation.Validate(value);
                    foreach (var error in errors)
                    {
                        SetError(error);
                    }
                }
                
                _value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasErrors));
            }
        }

        private bool _animate = false;

        public bool Animate
        {
            get { return _animate; }
            set
            {
                _animate = value;
                OnPropertyChanged();
            }
        }

        public async Task Animation()
        {
            Animate = true;
            await Task.Delay(1000);
            Animate = false;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        public bool HasErrors => _errors.Any();

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
                return null;
            return _errors[propertyName];
        }

        private void SetError(string error, [CallerMemberName] string propertyName = null)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors([CallerMemberName] string propertyName = null)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
