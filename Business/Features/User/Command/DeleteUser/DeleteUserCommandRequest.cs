using System;
using Business.Wrappers;
using MediatR;

namespace Business.Features.User.Command.DeleteUser;

public class DeleteUserCommandRequest : IRequest<Response>
{
    public string Id { get; set; }
}
