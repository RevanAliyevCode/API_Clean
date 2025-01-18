using System;
using Business.Features.User.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.User.Command.LoginUser;

public class LoginUserCommandRequest : IRequest<ResponseSuccess<TokenDTO>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

