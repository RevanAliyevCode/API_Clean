using System;
using Data.Repositeries.Base;
using E = Domain.Entities;

namespace Data.Repositeries.Product;

public interface IProductWriteRepository : IBaseWriteRepository<E.Product>
{

}
