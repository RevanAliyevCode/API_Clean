using System;
using AutoMapper;
using Business.Features.Product.Dtos;
using Business.Wrappers;
using Data.Repositeries.Product;
using Data.UnitOfWork;
using Domain.Exceptions;
using MediatR;

namespace Business.Features.Product.Command.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, ResponseSuccess<ProductDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IProductWriteRepository productRepository, IProductReadRepository productReadRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _productWriteRepository = productRepository;
        _productReadRepository = productReadRepository;
        _mapper = mapper;
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

        return new() { Data = _mapper.Map<ProductDTO>(existingProduct), Message = "Product updated successfully" };
    }
}
