using CoreBank.Application.Abstractions.Messaging;

namespace CoreBank.Application.Customers.Commands.VerifyCustomer;

public sealed record VerifyCustomerCommand(Guid CustomerId) : ICommand;