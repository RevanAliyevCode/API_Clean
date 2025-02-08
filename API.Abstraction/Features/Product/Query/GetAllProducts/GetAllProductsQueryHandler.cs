using System;
using API.Application.Features.Product.Dtos;
using API.Application.Wrappers;
using API.Domain.Repositories.Product;
using AutoMapper;
using MediatR;

namespace API.Application.Features.Product.Query.GetAllProducts;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, ResponseSuccess<List<ProductDTO>>>
{
    readonly IProductReadRepository _productReadRepository;
    readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IProductReadRepository productRepository, IMapper mapper)
    {
        _productReadRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ResponseSuccess<List<ProductDTO>>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
    {
        var products = await _productReadRepository.GetAllAsync();

        return new() { Data = _mapper.Map<List<ProductDTO>>(products) };
    }
}
