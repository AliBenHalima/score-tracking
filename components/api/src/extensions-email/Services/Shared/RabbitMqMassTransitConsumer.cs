using BenchmarkDotNet.Loggers;
using MassTransit;
using Microsoft.Extensions.Logging;
using Polly;
using ScoreTracking.Extensions.Email.Contracts;

namespace ScoreTracking.Extensions.Email.Services.Shared
{
    public class RabbitMqMassTransitConsumer : IConsumer<EmailMessage>
    {
        private readonly IEnumerable<IEmailSender> _emailSender;
        private readonly ILogger<RabbitMqMassTransitConsumer> _logger;

        public RabbitMqMassTransitConsumer(IEnumerable<IEmailSender> emailSender, ILogger<RabbitMqMassTransitConsumer> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<EmailMessage> context)
        {
            var email = context.Message;

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                 retryCount: 5,
                 retryAttempt =>
                 {
                     _logger.LogInformation("Retry attempt {retryAttempt}", retryAttempt);
                     return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                 });


            var circuitBreaker = Policy.Handle<Exception>()
                .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking: 2, TimeSpan.FromSeconds(30), (ex, timespan) => OnBreak(ex, timespan), OnReset());

            var fallbackPolicy = Policy.Handle<Exception>()
                     .FallbackAsync(async (cancellationToken) =>
                     await _emailSender.Skip(1).First().Send(email, cancellationToken) // Fake EmailSennder Implementation
                    );

            await fallbackPolicy.WrapAsync(retryPolicy).WrapAsync(circuitBreaker).ExecuteAsync(async () =>
            {
                _logger.LogInformation("Executig policy...");
                await _emailSender.First().Send(email);
                _logger.LogInformation("Email set successfully");
            });

        }

        private void OnBreak(Exception exception, TimeSpan timespan)
        {
            _logger.LogWarning("Circuit has been broken after {timespan}", timespan);

        }
        private Action OnReset()
        {
            {
                return () => _logger.LogInformation("Circuit has been restored");
            }
        }
    }
}
