using System;
using FluentValidation;

namespace API.Application.Features.User.Command.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommandRequest>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
