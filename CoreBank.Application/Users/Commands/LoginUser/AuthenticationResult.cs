namespace CoreBank.Application.Users.Commands.LoginUser;

public sealed record AuthenticationResult(
    string Token,
    Guid UserId,
    string Email,
    string FullName,
    string Role,
    DateTime ExpiresAt);
