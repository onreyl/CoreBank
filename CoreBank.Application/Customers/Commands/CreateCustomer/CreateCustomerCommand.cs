using CoreBank.Domain.Common;
using MediatR;

namespace CoreBank.Application.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    string Tckn,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Email
) : IRequest<Result<Guid>>;