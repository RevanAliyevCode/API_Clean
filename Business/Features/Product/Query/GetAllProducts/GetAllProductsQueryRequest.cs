using System;
using Business.Features.Product.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Product.Query.GetAllProducts;

public class GetAllProductsQueryRequest : IRequest<ResponseSuccess<List<ProductDTO>>>
{

}
