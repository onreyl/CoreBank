using CoreBank.Application.Abstractions.Messaging;
using CoreBank.Domain.Accounts;
using CoreBank.Domain.Common;
using CoreBank.Domain.Customers;
using CoreBank.Domain.Events;
using CoreBank.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreBank.Infrastructure.Persistence;

public class CoreBankDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public CoreBankDbContext(
        DbContextOptions<CoreBankDbContext> options,
        IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreBankDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await PublishDomainEventsAsync(cancellationToken);
        return result;
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var entitiesWithEvents = ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = entitiesWithEvents
            .SelectMany(entity => entity.DomainEvents)
            .ToList();

        foreach (var entity in entitiesWithEvents)
            entity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType());

            var notification = Activator.CreateInstance(notificationType, domainEvent)!;

            await _publisher.Publish(notification, cancellationToken);
        }
    }
}