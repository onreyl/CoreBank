using CoreBank.Domain.Common;

namespace CoreBank.Domain.Accounts
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
