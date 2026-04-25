using CoreBank.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreBank.Infrastructure.Persistence.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // 1. ToTable
            builder.ToTable("Customers");

            // 2. HasKey
            builder.HasKey(c => c.Id);

            // 3. Tckn (length + required + unique index)
            builder.Property(c => c.Tckn)
                .IsRequired()
                .HasMaxLength(11);
            builder.HasIndex(c => c.Tckn)
                .IsUnique();

            // 4. FirstName, LastName (length + required)
            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(100);

            // 5. PhoneNumber (length + required)
            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            // 6. Email (length + required + unique index)
            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasIndex(c => c.Email)
                .IsUnique();

            // 7. DateOfBirth (required)
            builder.Property(c => c.DateOfBirth)
                .IsRequired();

            // 8. Status (enum → string conversion + length)
            builder.Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();

            // 9. CreatedAtUtc (required), UpdatedAtUtc (nullable)
            builder.Property(c => c.CreatedAtUtc)
                .IsRequired();
            
            builder.Property(c => c.UpdatedAtUtc)
                .IsRequired(false);

            // 10. Ignore DomainEvents
            builder.Ignore(c => c.DomainEvents);
        }
    }
}
