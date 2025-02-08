using System;
using API.Application.Features.Role.Command.DeleteRoleFromUser;
using API.Domain.Entities;
using API.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace UnitTests.Handlers.Role.Command;

public class DeleteRoleFromUserTest
{
    readonly Mock<UserManager<AppUser>> _userManager;
    readonly Mock<RoleManager<IdentityRole>> _roleManager;
    readonly DeleteRoleFromUserCommandHandler _handler;

    public DeleteRoleFromUserTest()
    {
        _userManager = new Mock<UserManager<AppUser>>(new Mock<IUserStore<AppUser>>().Object, null, null, null, null, null, null, null, null);
        _roleManager = new Mock<RoleManager<IdentityRole>>(new Mock<IRoleStore<IdentityRole>>().Object, null, null, null, null);
        _handler = new DeleteRoleFromUserCommandHandler(_userManager.Object, _roleManager.Object);
    }

    [Fact]
    public async Task DeleteRoleFromUserCommandHandler_UserNotFound_ThrowNotFoundException()
    {
        // Arrange
        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((AppUser)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(new DeleteRoleFromUserCommandRequest(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task DeleteRoleFromUserCommandHandler_RoleNotFound_ThrowNotFoundException()
    {
        // Arrange
        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new AppUser());
        _roleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((IdentityRole)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(new DeleteRoleFromUserCommandRequest(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task DeleteRoleFromUserCommandHandler_RemoveRoleFailed_ThrowValidationException()
    {
        // Arrange
        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new AppUser());
        _roleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole("User"));
        _userManager.Setup(x => x.RemoveFromRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        // Act
        Func<Task> act = async () => await _handler.Handle(new DeleteRoleFromUserCommandRequest(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ValidationException>(act);
    }

    [Fact]
    public async Task DeleteRoleFromUserCommandHandler_Success_ReturnResponse()
    {
        // Arrange
        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new AppUser());
        _roleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole("User"));
        _userManager.Setup(x => x.RemoveFromRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(new DeleteRoleFromUserCommandRequest(), CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
    }

}
