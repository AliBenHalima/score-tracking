using ScoreTracking.Extensions.Email.Contratcs;

namespace ScoreTracking.Extensions.Email.Contracts
{
    public interface IEmailRepository
    {
        Task<int?> InsertIntoEmailQueueAsync(EmailMessage email);
        Task<EmailQueueEntity> RemoveLatestItemAsync();

        Task MarkEmailQueueAsProcessedAsync(string id);
        Task<bool> DatabaseHasItemAsync();
        Task MarkEmailQueueAsSucceededAsync(string id);
    }
}