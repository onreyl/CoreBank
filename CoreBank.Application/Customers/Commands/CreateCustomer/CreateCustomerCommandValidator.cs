using FluentValidation;

namespace CoreBank.Application.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandValidator
    : AbstractValidator<CreateCustomerCommand>
{
    private const string TurkishLetters = @"a-zA-ZçğıöşüÇĞİÖŞÜ";

    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Tckn)
            .NotEmpty()
            .Matches(@"^\d{11}$").WithMessage("TCKN 11 haneli rakam olmalı.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(2, 50)
            .Matches($@"^[{TurkishLetters} ]+$")
            .WithMessage("Ad sadece harf ve boşluk içerebilir.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(2, 50)
            .Matches($@"^[{TurkishLetters} ]+$")
            .WithMessage("Soyad sadece harf ve boşluk içerebilir.");

        RuleFor(x => x.DateOfBirth)
            .Must(BeAdult).WithMessage("Müşteri 18 yaşından büyük olmalı.")
            .Must(BeRealistic).WithMessage("Doğum tarihi gerçekçi olmalı.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^(\+90|0)?5\d{9}$")
            .WithMessage("Geçerli bir TR cep numarası giriniz.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200);
    }

    private static bool BeAdult(DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dob.Year;
        if (dob > today.AddYears(-age)) age--;
        return age >= 18;
    }

    private static bool BeRealistic(DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return dob > today.AddYears(-120) && dob <= today;
    }
}