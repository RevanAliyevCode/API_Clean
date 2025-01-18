using System;
using Business.DTOs.Role;
using Business.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Roles;

public class RoleService : IRoleService
{
    readonly RoleManager<IdentityRole> _roleManager;
    readonly UserManager<AppUser> _userManager;

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<ResponseSuccess<List<RoleDTO>>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();

        return new ResponseSuccess<List<RoleDTO>>() { Data = roles.Select(r => new RoleDTO() { Id = r.Id, Name = r.Name }).ToList() };
    }

    public async Task<Response> AddUserRoleAsync(string userId, RoleDTO role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new NotFoundException("User not found");

        var roleExists = await _roleManager.RoleExistsAsync(role.Name);

        if (!roleExists)
            throw new NotFoundException("Role not found");

        var result = await _userManager.AddToRoleAsync(user, role.Name);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description).ToList());

        return new Response() { Message = "Role added successfully", Succeeded = true };
    }

    public async Task<Response> DeleteUserRoleAsync(string userId, RoleDTO role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new NotFoundException("User not found");

        var roleExists = await _roleManager.RoleExistsAsync(role.Name);

        if (!roleExists)
            throw new NotFoundException("Role not found");

        var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description).ToList());

        return new Response() { Message = "Role deleted successfully", Succeeded = true };
    }

}
