using System;
using API.Application.Wrappers;
using API.Domain.Entities;
using API.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Application.Features.Role.Command.AddRoleToUser;

public class AddRoleToUserCommandHandler : IRequestHandler<AddRoleToUserCommandRequest, Response>
{
    readonly UserManager<AppUser> _userManager;
    readonly RoleManager<IdentityRole> _roleManager;

    public AddRoleToUserCommandHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Response> Handle(AddRoleToUserCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
            throw new NotFoundException("User not found");

        var roleExists = await _roleManager.FindByIdAsync(request.RoleId);

        if (roleExists?.Name == null)
            throw new NotFoundException("Role not found");

        var result = await _userManager.AddToRoleAsync(user, roleExists.Name);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description).ToList());

        return new Response() { Message = "Role added successfully", Succeeded = true };
    }
}
