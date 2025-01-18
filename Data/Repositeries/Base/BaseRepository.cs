using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositeries.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    readonly DbSet<T> _entities;

    public BaseRepository(AppDbContext context)
    {
        _entities = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
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
