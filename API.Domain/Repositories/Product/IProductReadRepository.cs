using System;
using API.Domain.Repositories.Base;
using E = API.Domain.Entities;

namespace API.Domain.Repositories.Product;

public interface IProductReadRepository : IBaseReadRepository<E.Product>
{
    Task<E.Product?> GetByNamesAsync(string name);
}
