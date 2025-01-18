using System;
using Business.Features.Product.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Product.Command.UpdateProduct;

public class UpdateProductCommandRequest : IRequest<ResponseSuccess<ProductDTO>>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Picture { get; set; }
    public int Stock { get; set; }
}
