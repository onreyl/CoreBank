using CoreBank.Application.Abstractions.Messaging;

namespace CoreBank.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FullName,
    string Role) : ICommand<RegisterUserResponse>;
