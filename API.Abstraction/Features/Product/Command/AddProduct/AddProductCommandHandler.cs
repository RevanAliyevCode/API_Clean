using System;
using API.Application.Abstraction.Services.Producer;
using API.Application.Abstraction.UnitOfWork;
using API.Application.Features.Product.Dtos;
using API.Application.Wrappers;
using API.Domain.Exceptions;
using API.Domain.Repositories.Product;
using AutoMapper;
using MediatR;
using E = API.Domain.Entities;

namespace API.Application.Features.Product.Command.AddProduct;

public class AddProductCommandHandler : IRequestHandler<AddProductCommandRequest, ResponseSuccess<ProductDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductWriteRepository _productRepository;
    readonly IMapper _mapper;
    readonly IProducerService _producerService;

    public AddProductCommandHandler(IUnitOfWork unitOfWork, IProductReadRepository productReadRepository, IProductWriteRepository productRepository, IMapper mapper, IProducerService producerService)
    {
        _unitOfWork = unitOfWork;
        _productReadRepository = productReadRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _producerService = producerService;
    }


    public async Task<ResponseSuccess<ProductDTO>> Handle(AddProductCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await new AddProductCommandValidator().ValidateAsync(request);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var existingProduct = await _productReadRepository.GetByNamesAsync(request.Name);

        if (existingProduct != null)
            throw new ValidationException("Product with the same name already exists");

        var newProduct = _mapper.Map<Domain.Entities.Product>(request);

        await _productRepository.AddAsync(newProduct);

        await _unitOfWork.CommitChangesAsync();

        await _producerService.ProduceAsync("create", newProduct);

        return new() { Data = _mapper.Map<ProductDTO>(newProduct), Message = "Product added successfully" };
    }
}
