namespace ScoreTracking.Extensions.Email.Contratcs;

public interface IEmailService {

    Task SendAsync(EmailMessage email, CancellationToken cancellationToken = default);
}