using ClusterSurveillance.Core;
using System;

namespace ClusterSurveillance.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand OptionViewCommand { get; set; }
        public RelayCommand CloseOptionViewCommand { get; set; }


        public RelayCommand TroubleshootViewCommand { get; set; }
        public RelayCommand CloseTroubleshootViewCommand { get; set; }

        public ServerViewModel ServerVM { get; set; }
        public OptionViewModel OptionVM { get; set; }
        public TroubleshootViewModel TroubleshootVM { get; set; }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }

        private object _optionView;

        public object OptionView
        {
            get { return _optionView; }
            set { _optionView = value;
                OnPropertyChanged();
            }
        }

        private bool _optionsIsChecked;

        public bool OptionsIsChecked
        {
            get { return _optionsIsChecked; }
            set { _optionsIsChecked = value;
                OnPropertyChanged();
            }
        }

        private bool _troublshootIsChecked;

        public bool TroubleshootIsChecked
        {
            get { return _troublshootIsChecked; }
            set { _troublshootIsChecked = value;
                OnPropertyChanged();
            }
        }



        public MainViewModel()
        {
            ServerVM = new ServerViewModel();
            OptionVM = new OptionViewModel();
            TroubleshootVM = new TroubleshootViewModel();

            CurrentView = ServerVM;

            OptionViewCommand = new RelayCommand(o => {
                OptionView = OptionVM;
                OptionsIsChecked = true;
            });
            CloseOptionViewCommand = new RelayCommand(o => {
                OptionView = null;
                OptionsIsChecked = false;
            });
            TroubleshootViewCommand = new RelayCommand(o =>
            {
                OptionView = TroubleshootVM;
                TroubleshootIsChecked = true;
            });
            CloseTroubleshootViewCommand = new RelayCommand(o =>
            {
                OptionView = null;
                TroubleshootIsChecked = false;
            });
        }
    }
}
