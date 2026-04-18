using Corebank.Domain.Common;

public class CurrencyMismatchException : DomainException
{
    public CurrencyMismatchException(string a, string b)
        : base($"Currency mismatch: {a} vs {b}") { }
}