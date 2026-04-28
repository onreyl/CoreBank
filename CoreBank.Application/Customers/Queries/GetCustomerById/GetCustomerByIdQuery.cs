using CoreBank.Application.Abstractions.Messaging;

namespace CoreBank.Application.Customers.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid CustomerId) : IQuery<CustomerDto>;