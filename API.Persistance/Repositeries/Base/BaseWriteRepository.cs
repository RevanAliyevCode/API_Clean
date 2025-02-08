using System;
using API.Domain.Entities;
using API.Domain.Repositories.Base;
using API.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace API.Persistance.Repositeries.Base;

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
