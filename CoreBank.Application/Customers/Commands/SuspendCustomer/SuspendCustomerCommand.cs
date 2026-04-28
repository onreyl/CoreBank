using CoreBank.Application.Abstractions.Messaging;

namespace CoreBank.Application.Customers.Commands.SuspendCustomer;

public sealed record SuspendCustomerCommand(Guid CustomerId) : ICommand;
