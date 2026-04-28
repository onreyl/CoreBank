using ErrorOr;
using MediatR;

namespace CoreBank.Application.Abstractions.Messaging;

public interface ICommand : IRequest<ErrorOr<Success>>
{
}

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>
{
}