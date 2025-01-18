using System;
using AutoMapper;
using Business.Features.Product.Command.AddProduct;
using Business.Features.Product.Command.UpdateProduct;
using Business.Features.Product.Dtos;
using E = Domain.Entities;

namespace Business.Mapping.Product;

public class MappingProduct : Profile
{
    public MappingProduct()
    {
        CreateMap<E.Product, ProductDTO>().ReverseMap();
    
        CreateMap<AddProductCommandRequest, E.Product>().ReverseMap();
    
        CreateMap<UpdateProductCommandRequest, E.Product>()
        .ForMember(dest => dest.Picture, opt =>
        {
            opt.Condition(src => src.Picture != null);
            opt.MapFrom(src => src.Picture);
        })
        .ReverseMap();
    }
}
