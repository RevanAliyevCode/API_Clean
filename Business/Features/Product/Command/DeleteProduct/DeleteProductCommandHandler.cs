using System;
using Business.Wrappers;
using Data.Repositeries.Product;
using Data.UnitOfWork;
using Domain.Exceptions;
using MediatR;

namespace Business.Features.Product.Command.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, Response>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IProductWriteRepository productRepository, IProductReadRepository productReadRepository)
    {
        _unitOfWork = unitOfWork;
        _productWriteRepository = productRepository;
        _productReadRepository = productReadRepository;
    }


    public async Task<Response> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
    {
        var existingProduct = await _productReadRepository.GetByIdAsync(request.Id);

        if (existingProduct == null)
            throw new NotFoundException("Product not found");

        _productWriteRepository.Delete(existingProduct);

        await _unitOfWork.CommitChangesAsync();

        return new() { Succeeded = true, Message = "Product deleted successfully" };
    }
}
