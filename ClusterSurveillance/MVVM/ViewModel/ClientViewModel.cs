using ClusterSurveillance.MVVM.Model;
using MQTTnet.Extensions.ManagedClient;

namespace ClusterSurveillance.MVVM.ViewModel
{
    public class ClientViewModel
    {
        public int TempLimit { get; set; }
        public int HumidLimit { get; set; }

        private IManagedMqttClient _mqttClient;
        private Client _client;
        public async void AcknowledgeAlarm()
        {
            await _mqttClient.PublishAsync($"ServerRoomCentral/confirm/{_client.ClientId}");
        }

        public async void SetLimits()
        {
            await _mqttClient.PublishAsync($"ServerRoomCentral/limits/{_client.ClientId}", $"{TempLimit}:{HumidLimit}");
        }
    }
}
