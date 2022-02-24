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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClusterSurveillance.MVVM.Model
{
    public class MqttConnector : ObservableObject
    {
        private readonly LoggerClass _logger;
        private readonly Config _config;

        public ObservableCollection<Client> Clients { get; set; }

        MqttClientOptionsBuilder _builder;
        ManagedMqttClientOptions _options;
        IManagedMqttClient _mqttClient;

        public Dictionary<int, string> StatusLevels = new Dictionary<int, string>()
        {
            [0] = "PATISSERIE CLOSED",
            [1] = "WAITING FOR BROKER",
            [2] = "PATISSERIE OPEN"
        };

        private int _status;

        public int Status
        {
            get { return _status; }
            set
            {
                _status = value;
                StatusMessage = StatusLevels[value];
                OnPropertyChanged();
            }
        }

        private string _statusMessage;

        public string StatusMessage
        {
            get { return _statusMessage; }
            set { _statusMessage = value;
                OnPropertyChanged();
            }
        }


        public MqttConnector(LoggerClass logger, Config config)
        {
            _logger = logger;
            _config = config;
            Clients = new ObservableCollection<Client>();
            App.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                Clients.Add(new Client("Test 1", "Spooky", "127.0.0.1", DateTime.Now));
                Clients.Add(new Client("Test 2", "Scary", "127.0.0.1", DateTime.Now));
                Clients.Add(new Client("Test 3", "Skeleton", null, DateTime.Now));
            });
        }

        public async void StartClientAsync()
        {
            _logger.Log(LogLevel.Information, "Starting mqtt client.");
            Status = 1;
            // Creates a new client
            _builder = new MqttClientOptionsBuilder()
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

            _mqttClient.UseApplicationMessageReceivedHandler(e => {
                if(!Clients.Any(client => client.ClientId == e.ClientId))
                {
                    App.Current.Dispatcher.BeginInvoke((Action)delegate
                    {
                        Clients.Add(new Client(e.ClientId, "", "", DateTime.Now));
                    });
                }
                else
                {
                    Clients.Where(client => string.Equals(client.ClientId, e.ClientId)).FirstOrDefault().GetMessage();
                }
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

        public void RestartClient()
        {
            StopClientAsync();
            StartClientAsync();
        }

        public async void StopClientAsync()
        {
            _logger.Log(LogLevel.Information, "Stopping running client");
            await _mqttClient.StopAsync();
        }

        public void OnConnected(MqttClientConnectedEventArgs obj)
        {
            _logger.Log(LogLevel.Information, "Successfully connected to broker.");
            Status = 2;
        }

        public void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            _logger.Log(LogLevel.Information, "Successfully disconnected from broker.");
            Status = 0;
        }

        public void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
        {
            _logger.Log(LogLevel.Warning, "Couldn't connect to broker.");
            if (!_config.AutoReconnectBroker)
            {
                _logger.Log(LogLevel.Information, "Stopping client.");
                _mqttClient.StopAsync();
                Status = 0;
            }
            else
            {
                _logger.Log(LogLevel.Information, $"Trying to reconnect in {_config.ReconnectTime} seconds");
                Status = 1;
            }
        }
    }
}
