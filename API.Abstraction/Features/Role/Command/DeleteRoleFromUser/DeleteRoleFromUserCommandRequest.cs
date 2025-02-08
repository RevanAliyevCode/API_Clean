using System;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.Role.Command.DeleteRoleFromUser;

public class DeleteRoleFromUserCommandRequest : IRequest<Response>
{
    public string UserId { get; set; }
    public string RoleId { get; set; }
}
