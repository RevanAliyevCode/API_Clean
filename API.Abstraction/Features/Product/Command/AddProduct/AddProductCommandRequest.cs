using System;
using API.Application.Features.Product.Dtos;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.Product.Command.AddProduct;

public class AddProductCommandRequest : IRequest<ResponseSuccess<ProductDTO>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Picture { get; set; }
    public int Stock { get; set; }
}
