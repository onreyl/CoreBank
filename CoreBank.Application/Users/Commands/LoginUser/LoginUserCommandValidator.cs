using FluentValidation;

namespace CoreBank.Application.Users.Commands.LoginUser;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email gereklidir")
            .EmailAddress().WithMessage("Email formatı geçersiz");

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Şifre gereklidir");
    }
}
