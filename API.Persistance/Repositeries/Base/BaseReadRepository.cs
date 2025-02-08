using System;
using API.Domain.Entities;
using API.Domain.Repositories.Base;
using API.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace API.Persistance.Repositeries.Base;

public class BaseReadRepository<T> : IBaseReadRepository<T> where T : BaseEntity
{
    readonly DbSet<T> _entities;

    public BaseReadRepository(AppDbContext context)
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
}
