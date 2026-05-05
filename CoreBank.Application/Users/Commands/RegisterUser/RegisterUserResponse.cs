namespace CoreBank.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserResponse(
    Guid UserId,
    string Email,
    string FullName,
    string Token,
    DateTime ExpiresAt);
