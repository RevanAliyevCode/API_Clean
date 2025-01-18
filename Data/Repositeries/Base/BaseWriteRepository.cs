using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositeries.Base;

public class BaseWriteRepository<T> : IBaseWriteRepository<T> where T : BaseEntity
{
    readonly DbSet<T> _entities;

    public BaseWriteRepository(AppDbContext context)
    {
        _entities = context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _entities.Update(entity);
    }

    public void Delete(T entity)
    {
        _entities.Remove(entity);
    }
}
