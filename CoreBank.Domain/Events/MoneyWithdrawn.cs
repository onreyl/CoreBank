namespace CoreBank.Domain.Events
{
    public sealed record MoneyWithdrawn(
        Guid AccountId,
        decimal Amount,
        string Currency,
        Guid EventId,
        DateTime OccurredOn
    ) : IDomainEvent;
}
