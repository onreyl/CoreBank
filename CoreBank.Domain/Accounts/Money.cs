namespace CoreBank.Domain.Accounts
{
    public sealed class Money
    {
        public decimal Amount { get; }
        public string Currency { get; }
        public Money(decimal amount, string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                throw new InvalidMoneyException("Currency cannot be empty");
            }
            if (currency.Length != 3)
            {
                throw new InvalidMoneyException("You entered the wrong currency");
            }
            if (amount < 0)
            {
                throw new InvalidMoneyException("Amount cannot be negative");
            }
            Currency = currency.ToUpper();
            Amount = amount;
        }

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
            {
                throw new CurrencyMismatchException(this.Currency, other.Currency);
            }
            decimal newAmount = Amount + other.Amount;
            return new Money(newAmount, Currency);
        }
        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
            {
                throw new CurrencyMismatchException(this.Currency, other.Currency);
            }
            decimal newAmount = Amount - other.Amount;
            return new Money(newAmount, Currency);
        }

        public static bool operator ==(Money? left, Money? right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Money? left, Money? right)
            => !(left == right);
        public override bool Equals(object? obj)
        {
            if (obj is not Money other)
                return false;

            return Amount == other.Amount && Currency == other.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
    }
}
