using System;
using AutoMapper;
using Business.Features.User.Command.LoginUser;
using Business.Features.User.Command.RegisterUser;
using Business.Features.User.Dtos;
using Domain.Entities;

namespace Business.Mapping.User;

public class MappingUser : Profile
{
    public MappingUser()
    {
        CreateMap<AppUser, UserDTO>().ReverseMap();

        CreateMap<RegisterUserCommandRequest, AppUser>().ReverseMap();

        CreateMap<LoginUserCommandRequest, AppUser>().ReverseMap();
    }
}
