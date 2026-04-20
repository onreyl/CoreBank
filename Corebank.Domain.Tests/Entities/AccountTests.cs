using Corebank.Domain.Events;
using CoreBank.Domain.Accounts;
using FluentAssertions;

namespace Corebank.Domain.Tests.Entities;

public class AccountTests
{
    [Fact]
    public void Open_Should_Raise_AccountOpened_Event()
    {
        // ---- Arrange ----
        var customerId = Guid.NewGuid();
        var currency = "TRY";

        // ---- Act ----
        var result = Account.Open(customerId, currency);

        // ---- Assert ----
        result.IsSuccess.Should().BeTrue();
        var account = result.Value;

        account.DomainEvents.Should().HaveCount(1);
        account.DomainEvents[0].Should().BeOfType<AccountOpened>();

        var accountOpened = (AccountOpened)account.DomainEvents[0];
        accountOpened.AccountId.Should().Be(account.Id);
        accountOpened.CustomerId.Should().Be(customerId);
        accountOpened.Currency.Should().Be(currency);
    }
}