using System;
using Data.Repositeries.Base;
using E = Domain.Entities;

namespace Data.Repositeries.Product;

public interface IProductReadRepository : IBaseReadRepository<E.Product>
{
    Task<E.Product?> GetByNamesAsync(string name);
}
