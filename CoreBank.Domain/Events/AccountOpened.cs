namespace Corebank.Domain.Events;

public sealed record AccountOpened(
    Guid AccountId,
    Guid CustomerId,
    string Currency,
    Guid EventId,
    DateTime OccurredOn
) : IDomainEvent;