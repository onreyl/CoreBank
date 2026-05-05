using CoreBank.Application.Abstractions.Authentication;
using CoreBank.Application.Abstractions.Messaging;
using CoreBank.Domain.Repositories;
using ErrorOr;

namespace CoreBank.Application.Users.Commands.LoginUser;

public sealed class LoginUserCommandHandler
    : ICommandHandler<LoginUserCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email, cancellationToken);

        if (user is null)
            return Error.Unauthorized(
                code: "Auth.InvalidCredentials",
                description: "Email veya şifre hatalı");

        if (!user.IsActive)
            return Error.Unauthorized(
                code: "Auth.InvalidCredentials",
                description: "Email veya şifre hatalı");

        var passwordValid = _passwordHasher.Verify(
            request.Password, user.PasswordHash);

        if (!passwordValid)
            return Error.Unauthorized(
                code: "Auth.InvalidCredentials",
                description: "Email veya şifre hatalı");

        user.RecordLogin();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            Token: token,
            UserId: user.Id,
            Email: user.Email,
            FullName: user.FullName,
            Role: user.Role,
            ExpiresAt: expiresAt);
    }
}
