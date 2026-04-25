using CoreBank.Domain.Accounts;
using CoreBank.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace CoreBank.Infrastructure.Persistence;

public class CoreBankDbContext : DbContext
{
    public CoreBankDbContext(DbContextOptions<CoreBankDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreBankDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}