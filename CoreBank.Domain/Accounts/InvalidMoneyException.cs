using Corebank.Domain.Common;

namespace Corebank.Domain.Accounts
{
    public class InvalidMoneyException : DomainException
    {
        public InvalidMoneyException(string message) : base(message)
        {
        }
        public InvalidMoneyException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
