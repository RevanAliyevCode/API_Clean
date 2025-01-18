using System;
using AutoMapper;
using Business.Features.Product.Dtos;
using Business.Wrappers;
using Data.Repositeries.Product;
using Data.UnitOfWork;
using Domain.Exceptions;
using MediatR;
using E = Domain.Entities;

namespace Business.Features.Product.Command.AddProduct;

public class AddProductCommandHandler : IRequestHandler<AddProductCommandRequest, ResponseSuccess<ProductDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductWriteRepository _productRepository;
    readonly IMapper _mapper;

    public AddProductCommandHandler(IUnitOfWork unitOfWork, IProductReadRepository productReadRepository, IProductWriteRepository productRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _productReadRepository = productReadRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }


    public async Task<ResponseSuccess<ProductDTO>> Handle(AddProductCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await new AddProductCommandValidator().ValidateAsync(request);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var existingProduct = await _productReadRepository.GetByNamesAsync(request.Name);

        if (existingProduct != null)
            throw new ValidationException("Product with the same name already exists");

        var newProduct = _mapper.Map<E.Product>(request);

        await _productRepository.AddAsync(newProduct);

        await _unitOfWork.CommitChangesAsync();

        return new() { Data = _mapper.Map<ProductDTO>(newProduct), Message = "Product added successfully" };
    }
}
