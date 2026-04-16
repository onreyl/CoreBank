namespace CoreBank.Domain.Accounts
{
    public class Money
    {
        public decimal Amount { get; }
        public string Currency { get; }
        public Money(decimal amount, string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentException("Currency cannot be empty");
            }
            if (currency.Length != 3)
            {
                throw new ArgumentException("You entered the wrong currency");
            }

            Currency = currency.ToUpper();
            Amount = amount;
        }

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
            {
                throw new InvalidOperationException("The currencies are not the same");
            }
            decimal newAmount = Amount + other.Amount;
            return new Money(newAmount, Currency);
        }
        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
            {
                throw new InvalidOperationException("The currencies are not the same");
            }
            decimal newAmount = Amount - other.Amount;
            return new Money(newAmount, Currency);
        }
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
