using System;
using API.Domain.Entities;

namespace API.Domain.Repositories.Base;

public interface IBaseWriteRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
