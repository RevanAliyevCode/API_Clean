using System;

namespace Business.DTOs.User;

public class TokenDTO
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
