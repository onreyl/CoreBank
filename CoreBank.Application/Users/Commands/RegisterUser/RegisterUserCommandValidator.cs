using FluentValidation;

namespace CoreBank.Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email gereklidir")
            .EmailAddress().WithMessage("Email formatı geçersiz")
            .MaximumLength(200);

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Şifre gereklidir")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalı")
            .MaximumLength(100);

        RuleFor(c => c.FullName)
            .NotEmpty().WithMessage("İsim gereklidir")
            .MinimumLength(2)
            .MaximumLength(200);

        RuleFor(c => c.Role)
            .NotEmpty()
            .Must(r => r == "Operator" || r == "Admin")
            .WithMessage("Geçersiz rol. Operator veya Admin olabilir.");
    }
}
