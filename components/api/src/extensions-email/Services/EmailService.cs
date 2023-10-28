using ScoreTracking.Extensions.Email.Contracts;


namespace ScoreTracking.Extensions.Email.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailQueue _emailQueue;

        public EmailService(IEmailQueue emailQueue)
        {
            _emailQueue = emailQueue;
        }

        public async Task SendAsync(EmailMessage email, CancellationToken cancellationToken = default)
        {
             await _emailQueue.Enqueue(email, cancellationToken);
        }
    }
}
