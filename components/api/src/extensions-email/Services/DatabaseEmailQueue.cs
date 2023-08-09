using AutoMapper;
using ScoreTracking.Extensions.Email.Contracts;
using ScoreTracking.Extensions.Email.Contratcs;
using ScoreTracking.Extensions.Email.Infrastructure;

namespace ScoreTracking.Extensions.Email.Services
{
    public class DatabaseEmailQueue : IEmailQueue
    {
        private readonly InitializeDatabase _database;
        private readonly IEmailRepository _emailRepository;
        private readonly IMapper _mapper;

        public DatabaseEmailQueue(IEmailRepository emailRepository, IMapper mapper, InitializeDatabase database)
        {

            _emailRepository = emailRepository;
            _mapper = mapper;
            _database = database;
        }

        public async Task Enqueue(EmailMessage email, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await _emailRepository.InsertIntoEmailQueueAsync(email);
        }

        public async Task<EmailMessage> GetAsync(CancellationToken cancellationToken = default)
        {
            var email = await _emailRepository.RemoveLatestItemAsync();

            await _emailRepository.MarkEmailQueueAsProcessedAsync(email.Id);
            EmailMessage emailMessage = _mapper.Map<EmailMessage>(email);
            if (emailMessage is not null)
            {
                return await Task.FromResult(emailMessage);
            }
            throw new InvalidOperationException();
        }

        public async Task<bool> HasItem(CancellationToken cancellationToken = default)
        {
            return await _emailRepository.DatabaseHasItemAsync();

        }

        public async Task MarkEmailQueueAsSucceededAsync(string emailId)
        {
            await _emailRepository.MarkEmailQueueAsSucceededAsync(emailId);
        }
    }
}


