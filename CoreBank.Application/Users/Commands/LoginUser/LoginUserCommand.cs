using CoreBank.Application.Abstractions.Messaging;

namespace CoreBank.Application.Users.Commands.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password) : ICommand<AuthenticationResult>;
