
namespace ScoreTracking.Extensions.Email.Contratcs;

public record EmailQueueEntity
{
    public string Id { get; init; }
    public string ReceiverName { get; init; }
    public string ReceiverAddress { get; init;}
    public string Content { get; init;}
    public string Subject { get; init; }
    public bool IsProcessed { get; init; }
    public bool? IsSuccessul { get; init; }
    public DateTimeOffset CreatedAt { get; init; }

}