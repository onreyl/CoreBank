using CoreBank.Domain.Accounts;
using CoreBank.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreBank.Infrastructure.Persistence.Configurations;

internal class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(x => x.Id);    

        builder.Property(x => x.AccountNumber)
               .HasMaxLength(16)
               .IsRequired();
        builder.HasIndex(x => x.AccountNumber).IsUnique();

        builder.Property(x => x.CustomerId).IsRequired();

        builder.HasOne<Customer>()
               .WithMany()
               .HasForeignKey(x => x.CustomerId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.OwnsOne(x => x.Balance, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("Balance_Amount")
                 .HasPrecision(18, 2)
                 .IsRequired();

            money.Property(m => m.Currency)
                 .HasColumnName("Balance_Currency")
                 .HasMaxLength(3)
                 .IsRequired();
        });

        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired(false);  

        builder.Ignore(x => x.DomainEvents);
    }
}