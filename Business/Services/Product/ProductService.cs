using System;
using AutoMapper;
using Business.DTOs.Product;
using Business.Validators.Product;
using Business.Wrappers;
using Data.Repositeries.Product;
using Data.UnitOfWork;
using Domain.Exceptions;
using E = Domain.Entities;

namespace Business.Services.Product;

public class ProductService : IProductService
{
    readonly IProductRepository _productRepository;
    readonly IMapper _mapper;
    readonly IUnitOfWork _unitOfWork;

    public ProductService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseSuccess<List<ProductDTO>>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return new() { Data = _mapper.Map<List<ProductDTO>>(products) };
    }

    public async Task<ResponseSuccess<ProductDTO>> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            throw new NotFoundException("Product not found");

        return new() { Data = _mapper.Map<ProductDTO>(product) };
    }

    public async Task<ResponseSuccess<ProductDTO>> AddProductAsync(ProductCreateDTO product)
    {
        var result = await new ProductCreateValidator().ValidateAsync(product);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var existingProduct = await _productRepository.GetByNamesAsync(product.Name);

        if (existingProduct != null)
            throw new ValidationException("Product with the same name already exists");

        var newProduct = _mapper.Map<E.Product>(product);

        await _productRepository.AddAsync(newProduct);

        await _unitOfWork.CommitChangesAsync();

        return new() { Data = _mapper.Map<ProductDTO>(newProduct) };
    }

    public async Task<ResponseSuccess<ProductDTO>> UpdateProductAsync(int id, ProductUpdateDTO product)
    {
        var result = await new ProductUpdateValidator().ValidateAsync(product);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var existingProduct = await _productRepository.GetByIdAsync(id);

        if (existingProduct == null)
            throw new NotFoundException("Product not found");

        _mapper.Map(product, existingProduct);

        _productRepository.Update(existingProduct);

        await _unitOfWork.CommitChangesAsync();

        return new() { Data = _mapper.Map<ProductDTO>(existingProduct) };
    }

    public async Task<Response> DeleteProductAsync(int id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);

        if (existingProduct == null)
            throw new NotFoundException("Product not found");

        _productRepository.Delete(existingProduct);

        await _unitOfWork.CommitChangesAsync();

        return new() { Succeeded = true, Message = "Product deleted successfully" };
    }
}
