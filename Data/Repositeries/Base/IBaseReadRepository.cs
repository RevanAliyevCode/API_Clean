using System;
using Domain.Entities;

namespace Data.Repositeries.Base;

public interface IBaseReadRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
}
