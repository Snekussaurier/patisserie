using ClusterSurveillance.Core;
using ClusterSurveillance.MVVM.ViewModel.Settings;

namespace ClusterSurveillance.MVVM.ViewModel
{
    internal class OptionViewModel : ObservableObject
    {
        public RelayCommand GeneralViewCommand { get; set; }
        public RelayCommand BrokerSettingsViewCommand { get; set; }
        public RelayCommand ExperimentalFeatureViewCommand { get; set; }
        public RelayCommand SoftwareUpdatesViewCommand { get; set; }

        public GeneralViewModel GeneralVM { get; set; }
        public BrokerSettingsViewModel BrokerSettingsVM { get; set; }
        public ExperimentalFeaturesViewModel ExperimentalFeaturesVM { get; set; }
        public SoftwareUpdatesViewModel SoftwareUpdatesVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { 
                _currentView = value;
                OnPropertyChanged();
            }
        }


        public OptionViewModel()
        {
            GeneralVM = new GeneralViewModel();
            BrokerSettingsVM = new BrokerSettingsViewModel();
            ExperimentalFeaturesVM = new ExperimentalFeaturesViewModel();
            SoftwareUpdatesVM = new SoftwareUpdatesViewModel();

            CurrentView = GeneralVM;

            GeneralViewCommand = new RelayCommand(o =>
            {
                CurrentView = GeneralVM;
            });

            BrokerSettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = BrokerSettingsVM;
            });

            ExperimentalFeatureViewCommand = new RelayCommand(o => { 
                CurrentView = ExperimentalFeaturesVM;
            });

            SoftwareUpdatesViewCommand = new RelayCommand(o =>
            {
                CurrentView = SoftwareUpdatesVM;
            });
        }
    }
}
