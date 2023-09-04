using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ScoreTracking.Extensions.Email.Contratcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ScoreTracking.Extensions.Email.Services
{
    public class RabbitMqEmailQueue : IEmailQueue, IDisposable
    {
        private IConnection _connection;
        public RabbitMqEmailQueue()
        {
            var factory = new ConnectionFactory { HostName = "localhost" }; // Using Local Broker
            _connection = factory.CreateConnection();
        }

        public Task Enqueue(EmailMessage email, CancellationToken cancellationToken = default)
        {
            using var channel = _connection.CreateModel();

            channel.QueueDeclare(queue: "email",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

            string serializedEmail = JsonConvert.SerializeObject(email);
            var body = Encoding.UTF8.GetBytes(serializedEmail);

            channel.BasicPublish(exchange: string.Empty,
                            routingKey: "email",
                            basicProperties: null,
                            body: body);
            return Task.CompletedTask;
        }

        public async Task<EmailMessage> GetAsync(CancellationToken cancellationToken = default)
        {
            using var channel = _connection.CreateModel();

            var result = channel.BasicGet("email", autoAck: false);  // autoAck set to false to not remove the message

            if (result is null)
            {
                throw new InvalidOperationException();
            }

            var body = result.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var emailMessage = JsonConvert.DeserializeObject<EmailMessage>(message); // Deserialize the object

            return await Task.FromResult(emailMessage);
        }

        public Task<bool> HasItem(CancellationToken cancellationToken = default)
        {
            using var channel = _connection.CreateModel();

            var queueDeclareOk = channel.QueueDeclare(queue: "email",
                       durable: false,
                       exclusive: false,
                       autoDelete: false,
                       arguments: null);

            var messageCount = queueDeclareOk.MessageCount; // Get Message Count ( Ready and Unaacked )

            var result = messageCount > 0 ? true : false;

            return Task.FromResult(result);
        }

        public Task MarkEmailQueueAsSucceededAsync(string? identifier)
        {
            // Return ACK to Delete Message from The Broker for good.
            using var channel = _connection.CreateModel();
            var result = channel.BasicGet("email", autoAck: false);
            channel.BasicAck(deliveryTag: result.DeliveryTag, multiple: false);
            return Task.CompletedTask;
        }


        public void Dispose()
        {
            _connection?.Dispose();
        }

    }
}
