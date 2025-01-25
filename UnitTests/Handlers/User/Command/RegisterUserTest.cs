using System;
using AutoMapper;
using Business.Features.User.Command.RegisterUser;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace UnitTests.Handlers.User.Command;

public class RegisterUserTest
{
    readonly Mock<UserManager<AppUser>> _userManager;
    readonly Mock<RoleManager<IdentityRole>> _roleManager;
    readonly Mock<IMapper> _mapper;

    readonly RegisterUserCommandHandler _handler;

    public RegisterUserTest()
    {
        var userStoreMock = new Mock<IUserStore<AppUser>>();
        var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();

        _userManager = new Mock<UserManager<AppUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        _roleManager = new Mock<RoleManager<IdentityRole>>(
            roleStoreMock.Object, null, null, null, null);

        _mapper = new Mock<IMapper>();

        _handler = new RegisterUserCommandHandler(_userManager.Object, _roleManager.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_WhenSomethingWrongWithCreateUser_ShouldThrowValidationException()
    {
        // Arrange
        var request = new RegisterUserCommandRequest()
        {
            Email = "some@email.com",
            Password = "Aa-12345",
            PasswordConfirmation = "Aa-12345"
        };

        _mapper.Setup(m => m.Map<AppUser>(request)).Returns(new AppUser());

        _userManager.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(act);
        Assert.Contains("Error", ex.Errors);
    }

    [Fact]
    public async Task Handle_WhenRoleNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var request = new RegisterUserCommandRequest()
        {
            Email = "some@email.com",
            Password = "Aa-12345",
            PasswordConfirmation = "Aa-12345"
        };

        _mapper.Setup(m => m.Map<AppUser>(request)).Returns(new AppUser());

        _userManager.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        _roleManager.Setup(r => r.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((IdentityRole)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Contains("Role not found", ex.Errors);
    }

    [Fact]
    public async Task Handle_WhenEverythingIsOk_ShouldReturnResponse()
    {
        // Arrange
        var request = new RegisterUserCommandRequest()
        {
            Email = "some@email.com",
            Password = "Aa-12345",
            PasswordConfirmation = "Aa-12345"
        };

        _mapper.Setup(m => m.Map<AppUser>(request)).Returns(new AppUser());

        _userManager.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        _roleManager.Setup(r => r.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole("User"));

        _userManager.Setup(u => u.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Succeeded);
        Assert.Equal("User registered successfully", response.Message);
    }
}
