using ScoreTracking.Extensions.Email.Contracts;

namespace ScoreTracking.Extensions.Email.Contracts;

public interface IEmailService {

    Task SendAsync(EmailMessage email, CancellationToken cancellationToken = default);
}