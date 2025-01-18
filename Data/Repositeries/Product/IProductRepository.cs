using System;
using Data.Repositeries.Base;
using E = Domain.Entities;

namespace Data.Repositeries.Product;

public interface IProductRepository : IBaseRepository<E.Product>
{
    Task<E.Product?> GetByNamesAsync(string name);
}
