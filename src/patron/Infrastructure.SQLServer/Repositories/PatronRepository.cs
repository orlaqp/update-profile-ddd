using System;
using Domain.Patron;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer.Repositories
{
    public class PatronRepository : Repository<Patron>, IPatronRepository
    {
        public PatronRepository(CommandsDbContext context) : base(context)
        {
        }

        public void Add(Patron patron) {
            Insert(patron);
        }

        public Patron GetById(Guid id)
        {
            return ById(id);
        }
    }
}