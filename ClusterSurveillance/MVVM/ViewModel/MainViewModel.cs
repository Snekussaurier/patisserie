using ClusterSurveillance.Core;
using ClusterSurveillance.MVVM.Model.Logger;
using ClusterSurveillance.MVVM.Model;
using Newtonsoft.Json;
using System.IO;

namespace ClusterSurveillance.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {

        public const string SERVER_TITLE = "Patisserie";
        public const string SETTINGS_TITLE = "Settings";
        public const string TROUBLESHOOT_TITLE = "Troubleshooting";

        public RelayCommand ServerViewCommand { get; set; }
        public RelayCommand LoggingViewCommand { get; set; }

        public RelayCommand OptionViewCommand { get; set; }
        public RelayCommand CloseOptionViewCommand { get; set; }
        public RelayCommand StopClientCommand { get; set; }


        public RelayCommand TroubleshootViewCommand { get; set; }
        public RelayCommand CloseTroubleshootViewCommand { get; set; }

        public ServerViewModel ServerVM { get; set; }
        public LoggingViewModel LoggingVM { get; set; }
        public OptionViewModel OptionVM { get; set; }
        public TroubleshootViewModel TroubleshootVM { get; set; }

        public LoggerClass Logger { get; set; }
        public MqttConnector Connector { get; set; }

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

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value;
                OnPropertyChanged();
            }
        }

        private Config config;

        public Config Config
        {
            get { return config; }
            set { config = value;
                OnPropertyChanged();
            }
        }



        public MainViewModel()
        {

            ServerVM = new ServerViewModel();
            OptionVM = new OptionViewModel();
            TroubleshootVM = new TroubleshootViewModel();
            LoggingVM = new LoggingViewModel();

            CurrentView = ServerVM;
            Title = SERVER_TITLE;

            OptionViewCommand = new RelayCommand(o => {
                OptionView = OptionVM;
                OptionsIsChecked = true;
                Title = SETTINGS_TITLE;
            });
            CloseOptionViewCommand = new RelayCommand(o => {
                OptionView = null;
                OptionsIsChecked = false;
                Title = SERVER_TITLE;
            });
            TroubleshootViewCommand = new RelayCommand(o =>
            {
                OptionView = TroubleshootVM;
                TroubleshootIsChecked = true;
                Title = TROUBLESHOOT_TITLE;
            });
            CloseTroubleshootViewCommand = new RelayCommand(o =>
            {
                OptionView = null;
                TroubleshootIsChecked = false;
                Title = SERVER_TITLE;
            });
            ServerViewCommand = new RelayCommand(o =>
            {
                CurrentView = ServerVM;
            });
            LoggingViewCommand = new RelayCommand(o =>
            {
                CurrentView = LoggingVM;
            });
            StopClientCommand = new RelayCommand(o =>
            {
                Connector.StopClientAsync();
            });

            Logger = new LoggerClass(LoggingVM);
            Config = new Config();
            try
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"Appsettings\appsettings.json"));
            }
            catch (System.Exception e)
            {
                Logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, e.Message);
            }
            Connector = new MqttConnector(Logger, Config);
            Connector.StartClientAsync();
        }
    }
}
