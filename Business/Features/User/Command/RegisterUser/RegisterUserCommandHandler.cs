using System;
using AutoMapper;
using Business.Features.User.Dtos;
using Business.Wrappers;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.User.Command.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommandRequest, Response>
{
    readonly UserManager<AppUser> _userManager;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly IMapper _mapper;

    public RegisterUserCommandHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<Response> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
    {
        var state = await new RegisterUserCommandValidator().ValidateAsync(request);

        if (!state.IsValid)
            throw new ValidationException(state.Errors);

        var user = _mapper.Map<AppUser>(request);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description).ToList());

        var role = await _roleManager.FindByNameAsync(UserRolesEnum.User.ToString());

        if (role?.Name == null)
            throw new NotFoundException("Role not found");

        await _userManager.AddToRoleAsync(user, role.Name);

        return new() { Message = "User registered successfully", Succeeded = true };
    }
}
