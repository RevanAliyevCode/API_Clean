using System;
using API.Application.Features.Role.Dtos;
using API.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Features.Role.Query.GetRolesQuery;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, ResponseSuccess<List<RoleDTO>>>
{
    readonly RoleManager<IdentityRole> _roleManager;

    public GetRolesQueryHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<ResponseSuccess<List<RoleDTO>>> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles.ToListAsync();

        return new ResponseSuccess<List<RoleDTO>>() { Data = roles.Select(r => new RoleDTO() { Id = r.Id, Name = r.Name }).ToList() };
    }
}
