using ScoreTracking.Extensions.Email.Contratcs;
using System.Collections.Concurrent;

namespace ScoreTracking.Extensions.Email.Services
{
    public class InMemoryEmailQueue : IEmailQueue
    {
        private readonly ConcurrentQueue<EmailMessage> _emailQueue = new ConcurrentQueue<EmailMessage>();
            
        public Task Enqueue(EmailMessage email, CancellationToken cancellationToken = default)
        {
            _emailQueue.Enqueue(email);
            return Task.CompletedTask;
        }

        public async Task<EmailMessage> GetAsync(CancellationToken cancellationToken = default)
        {
           var succeeded = _emailQueue.TryDequeue(out var result);
            if(succeeded && result is not null) {
                return await Task.FromResult(result);
            }
            throw new InvalidOperationException();
        }

        public Task<bool> HasItem(CancellationToken cancellationToken = default)
        {
           return Task.FromResult(_emailQueue.Count() > 0);
        }

        public Task MarkEmailQueueAsSucceededAsync(string emailId)
        {
            throw new NotImplementedException();
        }
    }
}
