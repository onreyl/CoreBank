namespace Corebank.Domain.Events
{
    public sealed record MoneyDeposited(
        Guid AccountId,
        decimal Amount,
        string Currency,
        Guid EventId,
        DateTime OccurredOn
    ) : IDomainEvent;
}
