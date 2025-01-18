using System;

namespace Business.DTOs.User;

public class RegisterUserDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
}
