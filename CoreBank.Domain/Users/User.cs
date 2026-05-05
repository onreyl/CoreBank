using CoreBank.Domain.Common;
using CoreBank.Domain.Events;

namespace CoreBank.Domain.Users
{
    public class User : Entity
    {
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public string FullName { get; private set; } = null!;
        public string Role { get; private set; } = null!;
        public bool IsActive { get; private set; }       
        public DateTime? LastLoginAtUtc { get; private set; }

        private User() { }

        private User(string email, string passwordHash, string fullName, string role)
        {
            Email = email;
            PasswordHash = passwordHash;
            FullName = fullName;
            Role = role;
            IsActive = true;
        }

        public static Result<User> Create(string email, string passwordHash, string fullName, string role)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result<User>.Failure("Email is required");

            email = email.Trim().ToLowerInvariant();

            if (!email.Contains('@'))
                return Result<User>.Failure("Email geçerli formatta olmalı");

            if (string.IsNullOrWhiteSpace(passwordHash))
                return Result<User>.Failure("Password is required");

            if (string.IsNullOrWhiteSpace(fullName))
                return Result<User>.Failure("Full name is required");

            if (role != UserRoles.Operator && role != UserRoles.Admin)
                return Result<User>.Failure("Geçersiz rol");

            var user = new User(email, passwordHash, fullName, role);

            user.RaiseDomainEvent(new UserCreated(
            UserId: user.Id,
            Email: user.Email,
            Role: user.Role,
            EventId: Guid.NewGuid(),
            OccurredOn: DateTime.UtcNow));

            return Result<User>.Success(user);
        }

        
        public Result ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                return Result.Failure("Password hash is required");

            PasswordHash = newPasswordHash;
            UpdatedAtUtc = DateTime.UtcNow;  // ← bunu da unutma
            return Result.Success();
        }

        public void RecordLogin()
        {
            LastLoginAtUtc = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }

    public static class UserRoles
    {
        public const string Operator = "Operator";
        public const string Admin = "Admin";
    }
}
