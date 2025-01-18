using System;
using Business.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.User.Command.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommandRequest, Response>
{
    readonly UserManager<AppUser> _userManager;

    public DeleteUserCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response> Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);

        if (user == null)
            throw new NotFoundException("User not found");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description).ToList());

        return new() { Message = "User deleted successfully", Succeeded = true };
    }
}
