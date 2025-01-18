using System;

namespace Business.DTOs.Product;

public class ProductUpdateDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string? Picture { get; set; }
    public int Stock { get; set; }
}
