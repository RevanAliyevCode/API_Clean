using System;
using API.Application.Features.User.Command.LoginUser;
using API.Application.Features.User.Command.RegisterUser;
using API.Application.Features.User.Dtos;
using API.Domain.Entities;
using AutoMapper;

namespace API.Application.Mapping.User;

public class MappingUser : Profile
{
    public MappingUser()
    {
        CreateMap<AppUser, UserDTO>().ReverseMap();

        CreateMap<RegisterUserCommandRequest, AppUser>().ReverseMap();

        CreateMap<LoginUserCommandRequest, AppUser>().ReverseMap();
    }
}
