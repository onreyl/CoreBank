namespace Corebank.Domain.Events;

public sealed record CustomerCreated(
    Guid CustomerId,
    string Tckn,
    string Email,
    Guid EventId,
    DateTime OccurredOn
) : IDomainEvent;
