using System;
using API.Application.Abstraction.Services.Producer;
using API.Application.Abstraction.UnitOfWork;
using API.Application.Wrappers;
using API.Domain.Exceptions;
using API.Domain.Repositories.Product;
using MediatR;

namespace API.Application.Features.Product.Command.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, Response>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    readonly IProducerService _producerService;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IProductWriteRepository productRepository, IProductReadRepository productReadRepository, IProducerService producerService)
    {
        _unitOfWork = unitOfWork;
        _productWriteRepository = productRepository;
        _productReadRepository = productReadRepository;
        _producerService = producerService;
    }


    public async Task<Response> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
    {
        var existingProduct = await _productReadRepository.GetByIdAsync(request.Id);

        if (existingProduct == null)
            throw new NotFoundException("Product not found");

        _productWriteRepository.Delete(existingProduct);

        await _unitOfWork.CommitChangesAsync();

        await _producerService.ProduceAsync("delete", existingProduct);

        return new() { Succeeded = true, Message = "Product deleted successfully" };
    }
}
