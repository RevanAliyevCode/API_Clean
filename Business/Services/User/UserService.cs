using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Business.DTOs.User;
using Business.Validators.User;
using Business.Wrappers;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services.User;

public class UserService : IUserService
{
    readonly UserManager<AppUser> _userManager;
    readonly SignInManager<AppUser> _signInManager;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly IMapper _mapper;
    readonly IConfiguration _configuration;

    public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<List<UserDTO>> GetUsersAsync()
    {
        var users = await _userManager.Users.Where(u => u.UserName != "admin@gmail.com").ToListAsync();

        return _mapper.Map<List<UserDTO>>(users);
    }

    public async Task<Response> RegisterUserAsync(RegisterUserDTO registerUserDTO)
    {
        var state = await new RegisterUserValidator().ValidateAsync(registerUserDTO);

        if (!state.IsValid)
            throw new ValidationException(state.Errors);

        var user = _mapper.Map<AppUser>(registerUserDTO);

        var result = await _userManager.CreateAsync(user, registerUserDTO.Password);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description).ToList());

        var role = await _roleManager.FindByNameAsync(UserRolesEnum.User.ToString());

        if (role?.Name == null)
            throw new NotFoundException("Role not found");

        await _userManager.AddToRoleAsync(user, role.Name);

        return new() { Message = "User registered successfully", Succeeded = true };
    }

    public async Task<TokenDTO> LoginUserAsync(LoginUserDTO loginUserDTO)
    {
        var state = await new LoginUserValidator().ValidateAsync(loginUserDTO);

        if (!state.IsValid)
            throw new ValidationException(state.Errors);

        var user = await _userManager.FindByEmailAsync(loginUserDTO.Email);

        if (user == null)
            throw new ValidationException("Invalid email or password");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserDTO.Password, false);

        if (!result.Succeeded)
            throw new ValidationException("Invalid email or password");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]) ?? throw new InvalidOperationException("JWT_SECRET is not set"));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenDTO { Token = tokenString, Expiration = token.ValidTo };
    }

    public async Task<Response> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User not found");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description).ToList());

        return new() { Message = "User deleted successfully", Succeeded = true };
    }
}