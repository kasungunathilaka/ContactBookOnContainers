using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderService.API.ViewModels;
using OrderService.API.Settings;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace OrderService.API.Messaging
{
    public class MessageQ : IMessageQ
    {
        private const string Server = "OMS_MQ_SERVER";
        private const string UserName = "OMS_MQ_USERNAME";
        private const string Password = "OMS_MQ_PASSWORD";
        readonly IOptions<MqSettings> _mqSettings;
        private readonly int _retryCount;
        private readonly ILogger<MessageQ> _logger;
        IConnection _connection;

        public MessageQ(IOptions<MqSettings> mqSettings, ILogger<MessageQ> logger, int retryCount = 5)
        {
            _mqSettings = mqSettings;
            _retryCount = retryCount;
            _logger = logger;
        }

        private IConnection CreateConnection()
        {
            var mqServer = Environment.GetEnvironmentVariable(Server);
            var mqUserName = Environment.GetEnvironmentVariable(UserName);
            var mqPassword = Environment.GetEnvironmentVariable(Password);

            var factory = new ConnectionFactory()
            {
                HostName = mqServer,
                UserName = mqUserName,
                Password = mqPassword
            };

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
               .Or<SocketException>()
               .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
               {
                   _logger.LogWarning(ex.ToString());
               });

            policy.Execute(() =>
            {
                _connection = factory.CreateConnection();
            });

            if(!IsConnected)
            {
                throw new InvalidOperationException("RabbitMQ connections could not be created and opened");
            }
            return _connection;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen;
            }
        }

        public void SendMessage(CustomerViewModel customer)
        {
            _connection = CreateConnection();

            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(_mqSettings.Value.ExchangeName, _mqSettings.Value.ExchangeType);

                channel.QueueDeclare(queue: _mqSettings.Value.QueueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueBind(queue: _mqSettings.Value.QueueName,
                                  exchange: _mqSettings.Value.ExchangeName,
                                  routingKey: _mqSettings.Value.RouteKey);

                string customerData = JsonConvert.SerializeObject(customer);
                var body = Encoding.UTF8.GetBytes(customerData);
              
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: _mqSettings.Value.ExchangeName,
                                        routingKey: _mqSettings.Value.RouteKey,
                                        basicProperties: properties,
                                        body: body);
                
            }
        }
    }
}
