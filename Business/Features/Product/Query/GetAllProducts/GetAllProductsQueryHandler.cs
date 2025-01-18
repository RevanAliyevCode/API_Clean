using System;
using AutoMapper;
using Business.Features.Product.Dtos;
using Business.Wrappers;
using Data.Repositeries.Product;
using MediatR;

namespace Business.Features.Product.Query.GetAllProducts;

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
