using CoreBank.Domain.Common;
using CoreBank.Domain.Customers;
using CoreBank.Domain.Repositories;
using MediatR;

namespace CoreBank.Application.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, Result<Guid>>
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

    public async Task<Result<Guid>> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
       
        var existingByTckn = await _customerRepository
            .GetByTcknAsync(request.Tckn, cancellationToken);
        if (existingByTckn is not null)
            return Result<Guid>.Failure("Bu TCKN ile kayıtlı müşteri zaten var");

        var existingByEmail = await _customerRepository
            .GetByEmailAsync(request.Email, cancellationToken);
        if (existingByEmail is not null)
            return Result<Guid>.Failure("Bu email ile kayıtlı müşteri zaten var");

        // Domain factory 
        var customerResult = Customer.Create(
            request.Tckn,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.PhoneNumber,
            request.Email);

        if (customerResult.IsFailure)
            return Result<Guid>.Failure(customerResult.Error);

        var customer = customerResult.Value;

        // Persist
        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(customer.Id);
    }
}