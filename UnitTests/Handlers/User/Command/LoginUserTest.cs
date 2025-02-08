using System;
using System.Net;
using API.Application.Features.User.Command.LoginUser;
using API.Domain.Entities;
using API.Domain.Exceptions;
using Business.Features.User.Command.LoginUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.Handlers.User.Command;

public class LoginUserTest
{
    readonly Mock<UserManager<AppUser>> _userManager;
    readonly Mock<SignInManager<AppUser>> _signInManager;
    readonly Mock<IConfiguration> _configuration;
    readonly LoginUserCommandHandler _handler;

    public LoginUserTest()
    {
        _userManager = new Mock<UserManager<AppUser>>(new Mock<IUserStore<AppUser>>().Object, null, null, null, null, null, null, null, null);
        _signInManager = new Mock<SignInManager<AppUser>>(_userManager.Object, new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object, null, null, null);
        _configuration = new Mock<IConfiguration>();
        _handler = new LoginUserCommandHandler(_userManager.Object, _signInManager.Object, _configuration.Object);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldValidationExcetion()
    {
        // Arrange
        var request = new LoginUserCommandRequest
        {
            Email = "test@test.com",
            Password = "password",
        };

        _userManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((AppUser)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ValidationException>(act);
    }

    [Fact]
    public async Task Handle_WhenPasswordIsInvalid_ShouldValidationExcetion()
    {
        // Arrange
        var request = new LoginUserCommandRequest
        {
            Email = "test@test.com",
            Password = "password",
        };

        var user = new AppUser
        {
            Id = "1",
            Email = request.Email,
        };

        _userManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _signInManager.Setup(x => x.CheckPasswordSignInAsync(user, request.Password, false)).ReturnsAsync(SignInResult.Failed);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ValidationException>(act);
    }

    [Fact]
    public async Task Handle_WhenUserFound_ShouldReturnToken()
    {
        // Arrange
        var request = new LoginUserCommandRequest
        {
            Email = "test@test.com",
            Password = "password",
        };

        var user = new AppUser
        {
            Id = "1",
            Email = request.Email,
        };

        _userManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
        _signInManager.Setup(x => x.CheckPasswordSignInAsync(user, request.Password, false)).ReturnsAsync(SignInResult.Success);

        _configuration.Setup(x => x["JWT:SecretKey"]).Returns("secretsecretsecretsecretsecretsecret");
        _configuration.Setup(x => x["JWT:Issuer"]).Returns("issuer");
        _configuration.Setup(x => x["JWT:Audience"]).Returns("audience");



        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Token);
        Assert.True(result.Data.Expiration > DateTime.Now);
    }
}
