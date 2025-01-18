using System;
using Business.DTOs.User;
using FluentValidation;

namespace Business.Validators.User;

public class LoginUserValidator : AbstractValidator<LoginUserDTO>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
