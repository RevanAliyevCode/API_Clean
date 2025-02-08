using System;
using API.Application.Features.User.Dtos;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.User.Command.LoginUser;

public class LoginUserCommandRequest : IRequest<ResponseSuccess<TokenDTO>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

