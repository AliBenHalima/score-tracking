using MassTransit;
using MassTransit.Testing;
using Polly.Caching;
using ScoreTracking.Extensions.Email.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.Extensions.Email.Services.Shared
{
    public class RabbitMqMassTransitProducer : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RabbitMqMassTransitProducer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T  : class
        {
            await _publishEndpoint.Publish(message);
        }
    }
}
