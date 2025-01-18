using System;
using Data.Repositeries.Base;
using Microsoft.EntityFrameworkCore;
using E = Domain.Entities;

namespace Data.Repositeries.Product;

public class ProductRepository : BaseRepository<E.Product>, IProductRepository
{
    readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<E.Product?> GetByNamesAsync(string name)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
    }
}
