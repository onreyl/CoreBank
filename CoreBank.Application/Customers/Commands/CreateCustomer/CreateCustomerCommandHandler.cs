using CoreBank.Application.Abstractions.Messaging;
using CoreBank.Domain.Customers;
using CoreBank.Domain.Repositories;
using ErrorOr;

namespace CoreBank.Application.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler
    : ICommandHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Guid>> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var existingByTckn = await _customerRepository
            .GetByTcknAsync(request.Tckn, cancellationToken);
        if (existingByTckn is not null)
            return Error.Conflict(
                code: "Customer.DuplicateTckn",
                description: "Bu TCKN ile kayıtlı müşteri zaten var");

        var existingByEmail = await _customerRepository
            .GetByEmailAsync(request.Email, cancellationToken);
        if (existingByEmail is not null)
            return Error.Conflict(
                code: "Customer.DuplicateEmail",
                description: "Bu email ile kayıtlı müşteri zaten var");

        var customerResult = Customer.Create(
            request.Tckn,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.PhoneNumber,
            request.Email);

        if (customerResult.IsFailure)
            return Error.Failure(
                code: "Customer.DomainRule",
                description: customerResult.Error);

        var customer = customerResult.Value;

        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}