using System;
using System.Text;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using ClusterSurveillance.MVVM.Model.Logger;
using Microsoft.Extensions.Logging;
using MQTTnet.Client.Receiving;

namespace ClusterSurveillance.MVVM.Model
{
    public class MqttConnector
    {
        private readonly LoggerClass _logger;

        public MqttConnector(LoggerClass logger)
        {
            _logger = logger;
        }

        public async void StartClient()
        {
            // Creates a new client
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                                                    .WithClientId("Dev.To")
                                                    .WithTcpServer("localhost", 1883);

            // Create client options objects
            ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(30))
                                    .WithClientOptions(builder.Build())
                                    .Build();

            // Creates the client object
            var _mqttClient = new MqttFactory().CreateManagedMqttClient();

            // Set up handlers
            _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
            _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);

            _mqttClient.UseApplicationMessageReceivedHandler(async e => {
                _logger.Log(LogLevel.Information, $"Message recieved: {e.ApplicationMessage.ConvertPayloadToString()}");
            });


            // Starts a connection with the Broker
            await _mqttClient.StartAsync(options);
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("sensorclient/data").Build());
        }

        public void OnConnected(MqttClientConnectedEventArgs obj)
        {
            _logger.Log(LogLevel.Information, "Successfully connected to broker.");
        }

        public void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            _logger.Log(LogLevel.Information, "Successfully disconnected from broker.");
        }

        public void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
        {
            _logger.Log(LogLevel.Warning, "Couldn't connect to broker.");
        }
    }
}
