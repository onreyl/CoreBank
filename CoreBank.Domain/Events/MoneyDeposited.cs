using CoreBank.Domain.Accounts;

namespace Corebank.Domain.Events
{
    public sealed record MoneyDeposited(
        Guid AccountId,
        Money Amount,
        Guid EventId,
        DateTime OccurredOn
    ) : IDomainEvent;
}
