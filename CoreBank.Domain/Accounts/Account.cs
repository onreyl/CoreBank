using Corebank.Domain.Common;

namespace Corebank.Domain.Accounts
{
    public class Account : Entity
    {
        public string AccountNumber { get; private set; } = null!;
        public Money Balance { get; private set; } = null!;
        public Guid CustomerId { get; private set; }

        private Account() { }

        private Account(Guid customerId, string accountNumber, Money balance)
        {
            CustomerId = customerId;
            AccountNumber = accountNumber;
            Balance = balance;
        }

        public static Result<Account> Open(Guid customerId, string currency)
        {
            if (customerId == Guid.Empty)
                return Result<Account>.Failure("CustomerId boş olamaz");

            try
            {
                var accountNumber = Guid.NewGuid().ToString("N")[..16].ToUpper();
                var balance = new Money(0, currency);  

                var account = new Account(customerId, accountNumber, balance);
                return Result<Account>.Success(account);
            }
            catch (DomainException ex)
            {
                return Result<Account>.Failure(ex.Message);
            }
        }

        public Result Deposit(Money amount)
        {
            if (amount.Amount <= 0)
                return Result.Failure("Deposit amount must be positive");

            try
            {
                Balance = Balance.Add(amount);
                UpdatedAt = DateTime.UtcNow;
                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public Result Withdraw(Money amount)
        {
            if (amount.Amount <= 0)
                return Result.Failure("Withdraw amount must be positive");
            if (Balance.Amount < amount.Amount)
                return Result.Failure("Insufficient balance");

            try
            {
                Balance = Balance.Subtract(amount);
                UpdatedAt = DateTime.UtcNow;
                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}