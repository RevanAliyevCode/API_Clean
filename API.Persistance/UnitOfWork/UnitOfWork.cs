using System;
using API.Application.Abstraction.UnitOfWork;
using API.Persistance.Contexts;


namespace API.Persistance.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
