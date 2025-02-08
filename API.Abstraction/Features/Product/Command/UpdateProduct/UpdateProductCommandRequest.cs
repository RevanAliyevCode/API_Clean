using System;
using API.Application.Features.Product.Dtos;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.Product.Command.UpdateProduct;

public class UpdateProductCommandRequest : IRequest<ResponseSuccess<ProductDTO>>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Picture { get; set; }
    public int Stock { get; set; }
}
