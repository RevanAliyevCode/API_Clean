using System;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Role.Command.DeleteRoleFromUser;

public class DeleteRoleFromUserCommandRequest : IRequest<Response>
{
    public string UserId { get; set; }
    public string RoleId { get; set; }
}
