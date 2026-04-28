using ErrorOr;
using MediatR;

namespace CoreBank.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>
{
}