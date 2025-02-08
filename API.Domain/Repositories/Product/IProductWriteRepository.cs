using System;
using API.Domain.Repositories.Base;
using E = API.Domain.Entities;

namespace API.Domain.Repositories.Product;

public interface IProductWriteRepository : IBaseWriteRepository<E.Product>
{

}
