namespace ScoreTracking.Extensions.Email.Contracts;

public interface IEmailQueue
{
    Task<EmailMessage> GetAsync(CancellationToken cancellationToken = default);
    Task Enqueue(EmailMessage email, CancellationToken cancellationToken = default);
    Task<bool> HasItem(CancellationToken cancellationToken = default);
    Task MarkEmailQueueAsSucceededAsync(string? identifier);
}