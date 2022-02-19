using System;
using System.Text;
using MQTTnet;
using MQTTnet.Server;
using ClusterSurveillance.MVVM.Model.Logger;
using Microsoft.Extensions.Logging;

namespace ClusterSurveillance.MVVM.Model
{
    public class Broker
    {
        private static int MessageCounter = 0;
        private readonly LoggerClass _logger;

        public Broker(LoggerClass logger)
        {
            _logger = logger;
        }

        public void StartServer()
        {
            Console.WriteLine("Initialize Broker");
            MqttServerOptionsBuilder options = new MqttServerOptionsBuilder()
                                        .WithDefaultEndpoint()
                                        .WithDefaultEndpointPort(707)
                                        .WithConnectionValidator(OnNewConnection)
                                        .WithApplicationMessageInterceptor(OnNewMessage);

            IMqttServer mqttServer = new MqttFactory().CreateMqttServer();

            mqttServer.StartAsync(options.Build()).GetAwaiter().GetResult();
        }

        public void OnNewConnection(MqttConnectionValidatorContext context)
        {
            Console.WriteLine(
                    $"New connection: ClientId = {context.ClientId}, Endpoint = {context.Endpoint}");
        }

        public void OnNewMessage(MqttApplicationMessageInterceptorContext context)
        {
            var payload = context.ApplicationMessage?.Payload == null ? null : Encoding.UTF8.GetString(context.ApplicationMessage?.Payload);

            MessageCounter++;
            if (context.ApplicationMessage.Topic == "sensorclient/alarm")
            {
                _logger.Log(LogLevel.Error,
                $"Message: ClientId = {context.ClientId}, Topic = {context.ApplicationMessage?.Topic}, Payload = {payload}, QoS = {context.ApplicationMessage?.QualityOfServiceLevel}, Retain-Flag = {context.ApplicationMessage?.Retain}");
                return;
            }
            _logger.Log(LogLevel.Information,
                $"Message: ClientId = {context.ClientId}, Topic = {context.ApplicationMessage?.Topic}, Payload = {payload}, QoS = {context.ApplicationMessage?.QualityOfServiceLevel}, Retain-Flag = {context.ApplicationMessage?.Retain}");
        }
    }
}
