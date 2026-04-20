using CoreBank.Domain.Customers;

namespace Corebank.Domain.Events;

public sealed record CustomerStatusChanged(
    Guid CustomerId,
    CustomerStatus OldStatus,
    CustomerStatus NewStatus,
    Guid EventId,
    DateTime OccurredOn
) : IDomainEvent;
