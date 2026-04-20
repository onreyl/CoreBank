using CoreBank.Domain.Accounts;

namespace Corebank.Domain.Events
{
    public sealed record MoneyWithdrawn(
        Guid AccountId,
        Money Amount,
        Guid EventId,
        DateTime OccurredOn
    ) : IDomainEvent;
   
}
