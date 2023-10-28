
namespace ScoreTracking.Extensions.Email.Contracts;

public class EmailMessage {
    public string Id { get; init; }
    public string ReceiverName { get; init; }
    public string ReceiverAddress { get; init;}
    public string Content { get; init;}
    public string Subject { get; init; }
}