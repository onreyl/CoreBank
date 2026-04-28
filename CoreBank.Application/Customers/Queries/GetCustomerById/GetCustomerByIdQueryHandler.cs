using CoreBank.Application.Abstractions.Messaging;
using CoreBank.Domain.Repositories;
using ErrorOr;

namespace CoreBank.Application.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryHandler
    : IQueryHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ErrorOr<CustomerDto>> Handle(
        GetCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(
            request.CustomerId,
            cancellationToken);

        if (customer is null)
            return Error.NotFound(
                code: "Customer.NotFound",
                description: "Müşteri bulunamadı");

        var dto = new CustomerDto(
            Id: customer.Id,
            Tckn: customer.Tckn,
            FirstName: customer.FirstName,
            LastName: customer.LastName,
            DateOfBirth: customer.DateOfBirth,
            PhoneNumber: customer.PhoneNumber,
            Email: customer.Email,
            Status: customer.Status.ToString(),
            CreatedAtUtc: customer.CreatedAtUtc,
            UpdatedAtUtc: customer.UpdatedAtUtc);

        return dto;
    }
}