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
using ClusterSurveillance.Core;

namespace ClusterSurveillance.MVVM.Model
{
    public class MqttConnector : ObservableObject
    {
        private readonly LoggerClass _logger;
        private readonly Config _config;

        MqttClientOptionsBuilder _builder;
        ManagedMqttClientOptions _options;
        IManagedMqttClient _mqttClient;

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value;
                OnPropertyChanged();
            }
        }

        public MqttConnector(LoggerClass logger, Config config)
        {
            _logger = logger;
            _config = config;
            Status = "OPENING PATISSERIE";
        }

        public async void StartClientAsync()
        {
            _logger.Log(LogLevel.Information, "Starting mqtt client.");
            // Creates a new client
           _builder = new MqttClientOptionsBuilder()
                                                    .WithClientId("Dev.To")
                                                    .WithTcpServer(_config.IPAdress, _config.Port) ;

            // Create client options objects
            _options = new ManagedMqttClientOptionsBuilder()
                                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(_config.ReconnectTime))
                                    .WithClientOptions(_builder.Build())
                                    .Build();

            // Creates the client object
            _mqttClient = new MqttFactory().CreateManagedMqttClient();

            // Set up handlers
            _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
            _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);

            _mqttClient.UseApplicationMessageReceivedHandler(async e => {
                if(e.ApplicationMessage.Topic == "sensorclient/alarm")
                {
                    _logger.Log(LogLevel.Error, $"Message recieved: {e.ApplicationMessage.ConvertPayloadToString()}");
                    return;
                }
                _logger.Log(LogLevel.Information, $"Message recieved: {e.ApplicationMessage.ConvertPayloadToString()}");
            });
            // Starts a connection with the Broker
            await _mqttClient.StartAsync(_options);
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("sensorclient/#").Build());
        }

        public async void RestartClient()
        {
            _logger.Log(LogLevel.Information, "Stopping running client");
            await _mqttClient.StopAsync();
            StartClientAsync();
        }

        public void OnConnected(MqttClientConnectedEventArgs obj)
        {
            _logger.Log(LogLevel.Information, "Successfully connected to broker.");
            Status = "PATISSERIE RUNNING";
        }

        public void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            _logger.Log(LogLevel.Information, "Successfully disconnected from broker.");
            Status = "PATISSERIE CLOSED";
        }

        public void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
        {
            _logger.Log(LogLevel.Warning, "Couldn't connect to broker.");
            if (!_config.AutoReconnectBroker)
            {
                _logger.Log(LogLevel.Information, "Stopping client.");
                _mqttClient.StopAsync();
                Status = "PATISSERIE CLOSED";
            }
            else
            {
                _logger.Log(LogLevel.Information, $"Trying to reconnect in {_config.ReconnectTime} seconds");
                Status = "WAITING FOR PASTRY";
            }
        }
    }
}
