using System;
using AutoMapper;
using Business.DTOs.User;
using Domain.Entities;

namespace Business.Mapping.User;

public class MappingUser : Profile
{
    public MappingUser()
    {
        CreateMap<AppUser, UserDTO>().ReverseMap();

        CreateMap<RegisterUserDTO, AppUser>().ReverseMap();

        CreateMap<LoginUserDTO, AppUser>().ReverseMap();
    }
}
