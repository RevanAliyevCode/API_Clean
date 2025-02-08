using System;
using API.Application.Features.Product.Dtos;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.Product.Query.GetProductById;

public class GetProductByIdQueryRequest : IRequest<ResponseSuccess<ProductDTO>>
{
    public int Id { get; set; }
}
