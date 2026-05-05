using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CoreBank.Application.Abstractions.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IErrorOr
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation(
            "Handling {RequestName}",
            requestName);

        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        if (response.IsError)
        {
            _logger.LogWarning(
                "{RequestName} returned errors in {ElapsedMs}ms. First error: {ErrorCode}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                response.Errors!.First().Code);
        }
        else
        {
            _logger.LogInformation(
                "{RequestName} handled successfully in {ElapsedMs}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}