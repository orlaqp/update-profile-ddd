using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
         DbContext Context { get; }
         Task<int> Commit();
         
    }
}