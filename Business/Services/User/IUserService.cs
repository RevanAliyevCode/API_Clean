using System;
using Business.DTOs.User;
using Business.Wrappers;

namespace Business.Services.User;

public interface IUserService
{
    Task<List<UserDTO>> GetUsersAsync();
    Task<Response> RegisterUserAsync(RegisterUserDTO registerUserDTO);
    Task<TokenDTO> LoginUserAsync(LoginUserDTO loginUserDTO);
    Task<Response> DeleteUserAsync(string id);
}
