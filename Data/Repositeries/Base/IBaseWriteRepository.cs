using System;
using Domain.Entities;

namespace Data.Repositeries.Base;

public interface IBaseWriteRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
