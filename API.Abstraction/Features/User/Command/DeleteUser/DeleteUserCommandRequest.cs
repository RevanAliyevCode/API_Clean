using System;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.User.Command.DeleteUser;

public class DeleteUserCommandRequest : IRequest<Response>
{
    public string Id { get; set; }
}
