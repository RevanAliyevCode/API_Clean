using System;

namespace API.Application.Features.User.Dtos;

public class TokenDTO
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
