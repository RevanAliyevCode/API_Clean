using System;
using Business.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.Role.Command.DeleteRoleFromUser;

public class DeleteRoleFromUserCommandHandler : IRequestHandler<DeleteRoleFromUserCommandRequest, Response>
{
    readonly UserManager<AppUser> _userManager;
    readonly RoleManager<IdentityRole> _roleManager;

    public DeleteRoleFromUserCommandHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Response> Handle(DeleteRoleFromUserCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
            throw new NotFoundException("User not found");

        var roleExists = await _roleManager.FindByIdAsync(request.RoleId);

        if (roleExists?.Name == null)
            throw new NotFoundException("Role not found");

        var result = await _userManager.RemoveFromRoleAsync(user, roleExists.Name);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description).ToList());

        return new Response() { Message = "Role deleted successfully", Succeeded = true };
    }
}
