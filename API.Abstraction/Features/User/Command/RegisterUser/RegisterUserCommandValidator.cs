using System;
using FluentValidation;

namespace API.Application.Features.User.Command.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommandRequest>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.PasswordConfirmation).Equal(x => x.Password);
    }
}
