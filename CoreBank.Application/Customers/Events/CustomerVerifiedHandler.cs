// CustomerVerifiedHandler.cs
using CoreBank.Application.Abstractions.Messaging;
using CoreBank.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoreBank.Application.Customers.Events;

public sealed class CustomerVerifiedHandler
    : INotificationHandler<DomainEventNotification<CustomerVerified>>
{
    private readonly ILogger<CustomerVerifiedHandler> _logger;

    public CustomerVerifiedHandler(ILogger<CustomerVerifiedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        DomainEventNotification<CustomerVerified> notification,
        CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;

        _logger.LogInformation(
            "CustomerVerified yakalandı. CustomerId: {CustomerId}, EventId: {EventId}, OccurredOn: {OccurredOn}",
            ev.CustomerId,
            ev.EventId,
            ev.OccurredOn);

        return Task.CompletedTask;
    }
}