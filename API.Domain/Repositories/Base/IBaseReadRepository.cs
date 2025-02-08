using System;
using API.Domain.Entities;

namespace API.Domain.Repositories.Base;

public interface IBaseReadRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
}
