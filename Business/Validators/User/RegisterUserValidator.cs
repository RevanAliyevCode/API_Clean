using System;
using Business.DTOs.User;
using FluentValidation;

namespace Business.Validators.User;

public class RegisterUserValidator : AbstractValidator<RegisterUserDTO>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.PasswordConfirmation).Equal(x => x.Password);
    }
}
