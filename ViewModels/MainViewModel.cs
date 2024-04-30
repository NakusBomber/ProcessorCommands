using ProcessorCommands.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
		private string _status;

		public string Status
		{
			get { return _status ?? "stop"; }
			set
			{
				_status = value;
				OnPropertyChanged();
			}
		}

		private RelayCommand _startCommand;

		public RelayCommand StartCommand
        {
			get
			{
				return _startCommand ??
					(_startCommand = new RelayCommand(
						obj =>
						{
							Status = "start";
						},
						(obj) => Status != "start"
						));
			}
		}

        private RelayCommand _stopCommand;

        public RelayCommand StopCommand
        {
            get
            {
                return _stopCommand ??
                    (_stopCommand = new RelayCommand(
                        obj =>
                        {
                            Status = "stop";
                        },
                        (obj) => Status != "stop"
                        ));
            }
        }


    }
}
