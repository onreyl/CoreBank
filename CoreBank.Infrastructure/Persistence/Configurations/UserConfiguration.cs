using CoreBank.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreBank.Infrastructure.Persistence.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // 1. ToTable
            builder.ToTable("Users");

            // 2. HasKey
            builder.HasKey(u => u.Id);

            // 3. Email (length + required + unique index)
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasIndex(u => u.Email)
                .IsUnique();

            // 4. PasswordHash (length + required)
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(200);

            // 5. FullName (length + required)
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(200);

            // 6. Role (length + required)
            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50);

            // 7. IsActive (required)
            builder.Property(u => u.IsActive)
                .IsRequired();

            // 8. LastLoginAtUtc (nullable)
            builder.Property(u => u.LastLoginAtUtc)
                .IsRequired(false);

            // 9. CreatedAtUtc (required), UpdatedAtUtc (nullable)
            builder.Property(u => u.CreatedAtUtc)
                .IsRequired();

            builder.Property(u => u.UpdatedAtUtc)
                .IsRequired(false);

            // 10. Ignore DomainEvents
            builder.Ignore(u => u.DomainEvents);
        }
    }
}