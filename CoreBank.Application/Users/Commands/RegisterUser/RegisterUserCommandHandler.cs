using CoreBank.Application.Abstractions.Authentication;
using CoreBank.Application.Abstractions.Messaging;
using CoreBank.Domain.Repositories;
using CoreBank.Domain.Users;
using ErrorOr;

namespace CoreBank.Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler
    : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
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

    public async Task<ErrorOr<RegisterUserResponse>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        // ─────────────────────────────────────────────────────
        // 1. Email Zaten Kayıtlı mı Kontrol Et
        // ─────────────────────────────────────────────────────
        var existing = await _userRepository.GetByEmailAsync(
            request.Email, cancellationToken);
        
        if (existing is not null)
            return Error.Conflict(
                code: "User.DuplicateEmail",
                description: "Bu email ile kayıtlı kullanıcı zaten var");

        // ─────────────────────────────────────────────────────
        // 2. Şifreyi Hash'le
        // ─────────────────────────────────────────────────────
        var passwordHash = _passwordHasher.Hash(request.Password);

        // ─────────────────────────────────────────────────────
        // 3. User Entity Oluştur
        // ─────────────────────────────────────────────────────
        var userResult = User.Create(
            email: request.Email,
            passwordHash: passwordHash,
            fullName: request.FullName,
            role: request.Role);

        if (userResult.IsFailure)
            return Error.Failure(
                code: "User.DomainRule",
                description: userResult.Error);

        var user = userResult.Value;

        // ─────────────────────────────────────────────────────
        // 4. Database'e Kaydet
        // ─────────────────────────────────────────────────────
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // ─────────────────────────────────────────────────────
        // 5. JWT Token Oluştur ← BURASI EKLENDİ!
        // ─────────────────────────────────────────────────────
        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

        // ─────────────────────────────────────────────────────
        // 6. Response Döndür
        // ─────────────────────────────────────────────────────
        var response = new RegisterUserResponse(
            UserId: user.Id,
            Email: user.Email,
            FullName: user.FullName,
            Token: token,
            ExpiresAt: expiresAt);

        return response;
    }
}
