using System;
using Business.Features.Role.Command.AddRoleToUser;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace UnitTests.Handlers.Role.Command;

public class AddToRoleTest
{
    readonly Mock<UserManager<AppUser>> _userManager;
    readonly Mock<RoleManager<IdentityRole>> _roleManager;
    readonly AddRoleToUserCommandHandler _handler;

    public AddToRoleTest()
    {
        _userManager = new Mock<UserManager<AppUser>>(new Mock<IUserStore<AppUser>>().Object, null, null, null, null, null, null, null, null);
        _roleManager = new Mock<RoleManager<IdentityRole>>(new Mock<IRoleStore<IdentityRole>>().Object, null, null, null, null);
        _handler = new AddRoleToUserCommandHandler(_userManager.Object, _roleManager.Object);
    }

    [Fact]
    public async Task AddRoleToUserCommandHandler_UserNotFound_ThrowNotFoundException()
    {
        // Arrange
        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((AppUser)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(new AddRoleToUserCommandRequest(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task AddRoleToUserCommandHandler_RoleNotFound_ThrowNotFoundException()
    {
        // Arrange
        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new AppUser());
        _roleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((IdentityRole)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(new AddRoleToUserCommandRequest(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task AddRoleToUserCommandHandler_AddRoleFailed_ThrowValidationException()
    {
        // Arrange
        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new AppUser());
        _roleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole("User"));
        _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        // Act
        Func<Task> act = async () => await _handler.Handle(new AddRoleToUserCommandRequest(), CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ValidationException>(act);
    }

    [Fact]
    public async Task AddRoleToUserCommandHandler_Success_ReturnResponse()
    {
        // Arrange
        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new AppUser());
        _roleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole("User"));
        _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(new AddRoleToUserCommandRequest(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.Equal("Role added successfully", result.Message);
    }
}
