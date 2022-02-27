using ClusterSurveillance.MVVM.Model;

namespace ClusterSurveillance.MVVM.ViewModel
{
    public class ServerViewModel
    {
        private MqttConnector _connector;
        public bool Visible { get; set; }
        public ServerViewModel(MqttConnector connector)
        {
            _connector = connector;
        }
    }
}
