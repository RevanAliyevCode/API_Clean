using System;
using Business.Features.Product.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Product.Command.AddProduct;

public class AddProductCommandRequest : IRequest<ResponseSuccess<ProductDTO>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Picture { get; set; }
    public int Stock { get; set; }
}
