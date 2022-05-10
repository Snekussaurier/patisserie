using ClusterSurveillance.Core;
using ClusterSurveillance.MVVM.Model;

namespace ClusterSurveillance.MVVM.ViewModel
{
    public class ServerViewModel : ObservableObject
    {
        private MqttConnector _connector;
        public bool Visible { get; set; }

        public RelayCommand ClientViewCommand { get; set; }
        public ClientViewModel ClientVM { get; set; }

        private object _clientView;

        public object ClientView
        {
            get { return _clientView; }
            set { _clientView = value;
                OnPropertyChanged();
            }
        }

        public ServerViewModel(MqttConnector connector)
        {
            _connector = connector;

            ClientViewCommand = new RelayCommand(o => {
                ClientView = new ClientViewModel();
            });
        }
    }
}
