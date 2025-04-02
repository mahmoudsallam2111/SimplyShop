namespace ServiceDefaults.Messaging.Events;

public record IntegrationEvent
{
    public Guid id => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName;
}
