using System;
using AutoMapper;
using Business.Features.User.Dtos;
using Business.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Features.User.Query.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQueryRequest, ResponseSuccess<List<UserDTO>>>
{
    readonly UserManager<AppUser> _userManager;
    readonly IMapper _mapper;

    public GetUsersQueryHandler(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }


    public async Task<ResponseSuccess<List<UserDTO>>> Handle(GetUsersQueryRequest request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.Where(u => u.UserName != "admin@gmail.com").ToListAsync();

        return new() { Data = _mapper.Map<List<UserDTO>>(users) };
    }
}
