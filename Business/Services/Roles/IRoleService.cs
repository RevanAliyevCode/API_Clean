using System;
using Business.DTOs.Role;
using Business.Wrappers;

namespace Business.Services.Roles;

public interface IRoleService
{
    public Task<ResponseSuccess<List<RoleDTO>>> GetRolesAsync();
    public Task<Response> AddUserRoleAsync(string userId, RoleDTO role);
    public Task<Response> DeleteUserRoleAsync(string userId, RoleDTO role);
}
