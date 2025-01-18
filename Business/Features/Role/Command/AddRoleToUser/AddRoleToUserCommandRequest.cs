using System;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Role.Command.AddRoleToUser;

public class AddRoleToUserCommandRequest : IRequest<Response>
{
    public string UserId { get; set; }
    public string RoleId { get; set; }
}
