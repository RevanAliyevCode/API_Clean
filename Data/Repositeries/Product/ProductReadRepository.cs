using System;
using Data.Repositeries.Base;
using Microsoft.EntityFrameworkCore;
using E = Domain.Entities;

namespace Data.Repositeries.Product;

public class ProductReadRepository : BaseReadRepository<E.Product>, IProductReadRepository
{
    readonly AppDbContext _context;
    public ProductReadRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<E.Product?> GetByNamesAsync(string name)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
    }
}
