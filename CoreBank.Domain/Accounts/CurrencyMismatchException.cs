using CoreBank.Domain.Common;

namespace CoreBank.Domain.Accounts
{
    public class CurrencyMismatchException : DomainException
    {
        public CurrencyMismatchException(string a, string b)
            : base($"Currency mismatch: {a} vs {b}") { }
    }
}