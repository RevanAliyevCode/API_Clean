using System;
using Business.Features.Product.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Product.Query.GetProductById;

public class GetProductByIdQueryRequest : IRequest<ResponseSuccess<ProductDTO>>
{
    public int Id { get; set; }
}
