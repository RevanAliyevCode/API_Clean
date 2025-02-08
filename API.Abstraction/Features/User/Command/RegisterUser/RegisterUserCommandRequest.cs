using System;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.User.Command.RegisterUser;

public class RegisterUserCommandRequest : IRequest<Response>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
}
