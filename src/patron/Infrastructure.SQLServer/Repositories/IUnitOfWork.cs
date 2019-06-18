using System;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
         DbContext Context { get; }
         void Commit();
         
    }
}