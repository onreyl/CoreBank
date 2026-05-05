using CoreBank.Domain.Users;

namespace CoreBank.Application.Abstractions.Authentication;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}