using System;
using AutoMapper;
using Business.DTOs.Product;
using E = Domain.Entities;

namespace Business.Mapping.Product;

public class MappingProduct : Profile
{
    public MappingProduct()
    {
        CreateMap<E.Product, ProductDTO>().ReverseMap();
    
        CreateMap<ProductCreateDTO, E.Product>().ReverseMap();
    
        CreateMap<ProductUpdateDTO, E.Product>()
        .ForMember(dest => dest.Picture, opt =>
        {
            opt.Condition(src => src.Picture != null);
            opt.MapFrom(src => src.Picture);
        })
        .ReverseMap();
    }
}
