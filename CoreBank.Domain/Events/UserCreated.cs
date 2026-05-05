namespace CoreBank.Domain.Events;

public sealed record UserCreated(
    Guid UserId,
    string Email,
    string Role,
    Guid EventId,
    DateTime OccurredOn) : IDomainEvent;
