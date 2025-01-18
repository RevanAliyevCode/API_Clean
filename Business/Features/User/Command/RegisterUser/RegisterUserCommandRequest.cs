using System;
using Business.Wrappers;
using MediatR;

namespace Business.Features.User.Command.RegisterUser;

public class RegisterUserCommandRequest : IRequest<Response>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
}
