using Microsoft.Extensions.Hosting;
using ScoreTracking.App.DTOs.Emails;
using ScoreTracking.App.Interfaces.Queues;
using ScoreTracking.App.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScoreTracking.App.BackgroundJobs.Jobs
{
    public class BackgroundEmailJob : BackgroundService
    {
        private readonly IEmailQueue _emailQueue;
        private readonly IEmailService _emailService;

        public BackgroundEmailJob(IEmailQueue emailQueue, IEmailService emailService)
        {
            _emailQueue = emailQueue;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
            {
                EmailDataDTO emailDataDTO = _emailQueue.DeQueueEmailTask();

                if (emailDataDTO is not null)
                {
                    _emailService.SendEmail(emailDataDTO);
                }
            }
            return Task.CompletedTask;
        }, stoppingToken);
        }
    }
}
