using System;
using API.Application.Features.Product.Dtos;
using API.Application.Wrappers;
using API.Domain.Exceptions;
using API.Domain.Repositories.Product;
using AutoMapper;
using MediatR;

namespace API.Application.Features.Product.Query.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, ResponseSuccess<ProductDTO>>
{
    readonly IProductReadRepository _productReadRepository;
    readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductReadRepository productRepository, IMapper mapper)
    {
        _productReadRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<ResponseSuccess<ProductDTO>> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetByIdAsync(request.Id);

        if (product == null)
            throw new NotFoundException("Product not found");

        return new() { Data = _mapper.Map<ProductDTO>(product) };
    }
}
