using System;
using Data.Repositeries.Base;
using E = Domain.Entities;

namespace Data.Repositeries.Product;

public class ProductWriteRepository : BaseWriteRepository<E.Product>, IProductWriteRepository
{
    public ProductWriteRepository(AppDbContext context) : base(context)
    {
    }
}
