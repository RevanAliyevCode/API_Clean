using System;
using Business.Features.User.Command.DeleteUser;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace UnitTests.Handlers.User.Command;

public class DeleteUserTest
{
    readonly Mock<UserManager<AppUser>> _userManager;
    readonly DeleteUserCommandHandler _handler;

    public DeleteUserTest()
    {
        _userManager = new Mock<UserManager<AppUser>>(new Mock<IUserStore<AppUser>>().Object, null, null, null, null, null, null, null, null);
        _handler = new DeleteUserCommandHandler(_userManager.Object);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var request = new DeleteUserCommandRequest
        {
            Id = "1"
        };

        _userManager.Setup(x => x.FindByIdAsync(request.Id)).ReturnsAsync((AppUser)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task Handle_WhenDeleteUserFailed_ShouldThrowValidationException()
    {
        // Arrange
        var request = new DeleteUserCommandRequest
        {
            Id = "1"
        };

        var user = new AppUser
        {
            Id = request.Id
        };

        _userManager.Setup(x => x.FindByIdAsync(request.Id)).ReturnsAsync(user);
        _userManager.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(act);
        Assert.Contains("Error", ex.Errors);
    }

    [Fact]
    public async Task Handle_WhenDeleteUserSucceeded_ShouldReturnResponse()
    {
        // Arrange
        var request = new DeleteUserCommandRequest
        {
            Id = "1"
        };

        var user = new AppUser
        {
            Id = request.Id
        };

        _userManager.Setup(x => x.FindByIdAsync(request.Id)).ReturnsAsync(user);
        _userManager.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Succeeded);
        Assert.Equal("User deleted successfully", response.Message);
    }
}
