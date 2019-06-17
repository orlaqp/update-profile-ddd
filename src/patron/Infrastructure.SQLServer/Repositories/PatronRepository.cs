using System;
using Domain.Patron;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer.Repositories
{
    public class PatronRepository : Repository<Patron>, IPatronRepository
    {
        public PatronRepository(DbContext context) : base(context)
        {
        }

        public Patron GetById(Guid id)
        {
            return ById(id);
        }
    }
}