using System;
using API.Domain.Repositories.Product;
using API.Persistance.Contexts;
using API.Persistance.Repositeries.Base;
using E = API.Domain.Entities;

namespace API.Persistance.Repositeries.Product;

public class ProductWriteRepository : BaseWriteRepository<E.Product>, IProductWriteRepository
{
    public ProductWriteRepository(AppDbContext context) : base(context)
    {
    }
}
