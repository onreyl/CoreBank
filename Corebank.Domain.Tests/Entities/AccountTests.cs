using CoreBank.Domain.Events;
using CoreBank.Domain.Accounts;
using CoreBank.Domain.Customers;
using FluentAssertions;

namespace CoreBank.Domain.Tests.Entities;

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

    [Fact]
    public void Deposit_Should_Raise_MoneyDeposited_Event()
    {
        var account = Account.Open(Guid.NewGuid(), "TRY").Value;

        var result = account.Deposit(new Money(100, "TRY"));
        result.IsSuccess.Should().BeTrue();

        account.DomainEvents.Should().HaveCount(2);
        account.DomainEvents[1].Should().BeOfType<MoneyDeposited>();

        var moneyDeposited = (MoneyDeposited)account.DomainEvents[1];
        moneyDeposited.AccountId.Should().Be(account.Id);
        moneyDeposited.Amount.Should().Be(100m);
        moneyDeposited.Currency.Should().Be("TRY");

    }

    [Fact]
    public void Withdraw_Should_Raise_MoneyWithdrawn_Event()
    {
        var account = Account.Open(Guid.NewGuid(), "TRY").Value;
        account.Deposit(new Money(100, "TRY"));

        var result = account.Withdraw(new Money(50, "TRY"));
        result.IsSuccess.Should().BeTrue();

        account.DomainEvents.Should().HaveCount(3);
        account.DomainEvents[2].Should().BeOfType<MoneyWithdrawn>();

        var moneyWithdrawn = (MoneyWithdrawn)account.DomainEvents[2];
        moneyWithdrawn.AccountId.Should().Be(account.Id);
        moneyWithdrawn.Amount.Should().Be(50m);
        moneyWithdrawn.Currency.Should().Be("TRY");
    }
}