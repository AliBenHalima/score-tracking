using Microsoft.Extensions.Logging;
using ScoreTracking.App.BackgroundJobs.Jobs;
using ScoreTracking.Extensions.Email.Contracts;
using ScoreTracking.Extensions.Email.Contratcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.Extensions.Email.Services
{
    public class FakeEmailSender : IEmailSender
    {
        private readonly ILogger<FakeEmailSender> _logger;

        public FakeEmailSender(ILogger<FakeEmailSender> logger)
        {
            _logger = logger;
        }

        public Task Send(EmailMessage message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending email to Fake {TO}", message.ReceiverAddress);
            return Task.CompletedTask;
        }
    }
}
