using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using ScoreTracking.Extensions.Email.Contracts;
using ScoreTracking.Extensions.Email.Contratcs;

namespace ScoreTracking.App.BackgroundJobs.Jobs
{
    public class BackgroundEmailWorker : BackgroundService
    {
        private readonly IEmailQueue _emailQueue;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<BackgroundEmailWorker> _logger;

        public BackgroundEmailWorker(IEmailQueue emailQueue, ILogger<BackgroundEmailWorker> logger, IEmailSender emailSender)
        {
            _emailQueue = emailQueue;
            _logger = logger;
            _emailSender = emailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting Email Logger");

            try
            {

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {

                        var hasItem = await _emailQueue.HasItem(stoppingToken);
                        if (hasItem)
                        {
                            var email = await _emailQueue.GetAsync(stoppingToken);


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

                            var policy = retryPolicy.WrapAsync(circuitBreaker);

                            await policy.ExecuteAsync(async () =>
                            {
                                _logger.LogInformation("Executig policy...");
                                await _emailSender.Send(email, stoppingToken);
                                _logger.LogInformation("Email set successfully");
                                await _emailQueue.MarkEmailQueueAsSucceededAsync(email.Id);
                            });
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }

            }
            finally
            {
                _logger.LogInformation("Stopping Email Logger");
            }

        }

        private Task LogInfo(string info, params object[] args)
        {
            _logger.LogInformation(info);
            return Task.CompletedTask;
        }
        private void OnBreak(Exception exception, TimeSpan timespan)
        {
            _logger.LogWarning("Circuit has been broken after {timespan}", timespan);
            _logger.LogError("Circuit Break Error", exception.Message);
        }
        private Action OnReset()
        {
            {
                return () => _logger.LogInformation("Circuit has been restored");
            }
        }
    }
}
