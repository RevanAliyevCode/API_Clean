using System;
using API.Application.Abstraction.Services.Producer;
using API.Application.Abstraction.UnitOfWork;
using API.Application.Features.Product.Dtos;
using API.Application.Wrappers;
using API.Domain.Exceptions;
using API.Domain.Repositories.Product;
using AutoMapper;
using MediatR;

namespace API.Application.Features.Product.Command.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, ResponseSuccess<ProductDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IMapper _mapper;
    readonly IProducerService _producerService;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IProductWriteRepository productRepository, IProductReadRepository productReadRepository, IMapper mapper, IProducerService producerService)
    {
        _unitOfWork = unitOfWork;
        _productWriteRepository = productRepository;
        _productReadRepository = productReadRepository;
        _mapper = mapper;
        _producerService = producerService;
    }

    public async Task<ResponseSuccess<ProductDTO>> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await new UpdateProductCommandValidator().ValidateAsync(request);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var existingProduct = await _productReadRepository.GetByIdAsync(request.Id);

        if (existingProduct == null)
            throw new NotFoundException("Product not found");

        _mapper.Map(request, existingProduct);

        _productWriteRepository.Update(existingProduct);

        await _unitOfWork.CommitChangesAsync();

        await _producerService.ProduceAsync("update", existingProduct);

        return new() { Data = _mapper.Map<ProductDTO>(existingProduct), Message = "Product updated successfully" };
    }
}
