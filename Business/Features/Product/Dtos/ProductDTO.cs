using System;

namespace Business.Features.Product.Dtos;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Picture { get; set; }
    public int Stock { get; set; }
}
