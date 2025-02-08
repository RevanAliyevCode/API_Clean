using System;
using API.Application.Features.Product.Command.AddProduct;
using API.Application.Features.Product.Command.UpdateProduct;
using API.Application.Features.Product.Dtos;
using AutoMapper;
using E = API.Domain.Entities;

namespace API.Application.Mapping.Product;

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
