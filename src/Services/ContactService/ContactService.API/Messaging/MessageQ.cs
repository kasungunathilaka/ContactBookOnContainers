using ContactService.API.ContactProviders;
using ContactService.API.Settings;
using ContactService.API.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.API.Messaging
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
        private readonly IContactsService _contactService;

        public MessageQ(IOptions<MqSettings> mqSettings, ILogger<MessageQ> logger, IContactsService contactsService, int retryCount = 5)
        {
            _mqSettings = mqSettings;
            _retryCount = retryCount;
            _logger = logger;
            _contactService = contactsService;
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

            if (!IsConnected)
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

        public void ConsumeMessage()
        {
            try
            {
                var _connection = CreateConnection();

                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(_mqSettings.Value.ExchangeName, _mqSettings.Value.ExchangeType);

                    channel.QueueDeclare(queue: _mqSettings.Value.QueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    channel.QueueBind(queue: _mqSettings.Value.QueueName,
                                      exchange: _mqSettings.Value.ExchangeName,
                                      routingKey: _mqSettings.Value.RouteKey);

                    var consumer = new EventingBasicConsumer(channel);

                    BasicGetResult result = channel.BasicGet(_mqSettings.Value.QueueName, true);
                    if (result != null)
                    {
                        string message = Encoding.UTF8.GetString(result.Body);
                        var customer = JsonConvert.DeserializeObject<CustomerViewModel>(message);
                        UpdateContacts(customer);
                    }

                    channel.BasicConsume(queue: _mqSettings.Value.QueueName,
                                            autoAck: false,
                                            consumer: consumer);
                 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred while consuming message: {ex.Message}");
            }
            
        }

        private void UpdateContacts(CustomerViewModel customer)
        {
            ContactViewModel contactViewModel = new ContactViewModel()
            {
                CustomerId = customer.CustomerId.ToString(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Gender = customer.Gender,
                Email = customer.Email,
                MobilePhone = customer.MobilePhone,
                HomePhone = customer.HomePhone,
                FacebookId = customer.FacebookId,
                ContactDetailsId = customer.ContactDetailsId.ToString(),
                Street = customer.Addresses.ElementAt(0) != null ? customer.Addresses.ElementAtOrDefault(0).Street : null,
                City = customer.Addresses.ElementAt(0) != null ? customer.Addresses.ElementAtOrDefault(0).City : null,
                Province = customer.Addresses.ElementAt(0) != null ? customer.Addresses.ElementAtOrDefault(0).Province : null,
                ZipCode = customer.Addresses.ElementAt(0) != null ? customer.Addresses.ElementAtOrDefault(0).ZipCode : null
            };

            _contactService.AddContact(contactViewModel);
        }
    }       
}
