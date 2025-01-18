using System;
using Business.DTOs.Product;
using Business.Wrappers;


namespace Business.Services.Product;

public interface IProductService
{
    Task<ResponseSuccess<List<ProductDTO>>> GetAllProductsAsync();
    Task<ResponseSuccess<ProductDTO>> GetProductByIdAsync(int id);
    Task<ResponseSuccess<ProductDTO>> AddProductAsync(ProductCreateDTO product);
    Task<ResponseSuccess<ProductDTO>> UpdateProductAsync(int id, ProductUpdateDTO product);
    Task<Response> DeleteProductAsync(int id);
}
