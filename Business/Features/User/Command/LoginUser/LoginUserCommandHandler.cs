using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Features.User.Dtos;
using Business.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Features.User.Command.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, ResponseSuccess<TokenDTO>>
{
    readonly UserManager<AppUser> _userManager;
    readonly SignInManager<AppUser> _signInManager;
    readonly IConfiguration _configuration;

    public LoginUserCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<ResponseSuccess<TokenDTO>> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
    {
        var state = await new LoginUserCommandValidator().ValidateAsync(request);

        if (!state.IsValid)
            throw new ValidationException(state.Errors);

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new ValidationException("Invalid email or password");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            throw new ValidationException("Invalid email or password");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]) ?? throw new InvalidOperationException("JWT_SECRET is not set"));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        var accessToken = new TokenDTO { Token = tokenString, Expiration = token.ValidTo };

        return new() { Data = accessToken, Message = "User logged in successfully" };
    }
}
