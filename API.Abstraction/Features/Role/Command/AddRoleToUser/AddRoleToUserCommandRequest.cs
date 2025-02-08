using System;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.Role.Command.AddRoleToUser;

public class AddRoleToUserCommandRequest : IRequest<Response>
{
    public string UserId { get; set; }
    public string RoleId { get; set; }
}
