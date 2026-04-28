using CoreBank.Application.Abstractions.Messaging;
using CoreBank.Domain.Repositories;
using ErrorOr;

namespace CoreBank.Application.Customers.Commands.VerifyCustomer;

public sealed class VerifyCustomerCommandHandler
    : ICommandHandler<VerifyCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Success>> Handle(
        VerifyCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(
            request.CustomerId,
            cancellationToken);

        if (customer is null)
            return Error.NotFound(
                code: "Customer.NotFound",
                description: "Müşteri bulunamadı");

        var domainResult = customer.Verify();
        if (domainResult.IsFailure)
            return Error.Conflict(
                code: "Customer.InvalidState",
                description: domainResult.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}