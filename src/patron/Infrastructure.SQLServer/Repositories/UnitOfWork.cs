using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(CommandsDbContext context)
        {
            Context = context;
        }

        public DbContext Context { get; }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}