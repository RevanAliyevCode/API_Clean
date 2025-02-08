using System;
using API.Application.Features.Product.Dtos;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.Product.Query.GetAllProducts;

public class GetAllProductsQueryRequest : IRequest<ResponseSuccess<List<ProductDTO>>>
{

}
