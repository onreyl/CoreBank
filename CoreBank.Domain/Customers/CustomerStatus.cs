namespace CoreBank.Domain.Customers;

public enum CustomerStatus
{
    PendingVerification = 0,
    Active = 1,
    Passive = 2,
    Closed = 3
}