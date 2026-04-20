namespace Corebank.Domain.Events;

public sealed record CustomerVerified(
    Guid CustomerId,
    Guid EventId,
    DateTime OccurredOn
) : IDomainEvent;
