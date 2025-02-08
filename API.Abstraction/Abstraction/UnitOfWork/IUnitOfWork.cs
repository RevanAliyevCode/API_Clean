using System;

namespace API.Application.Abstraction.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> CommitChangesAsync();
}
