using System;
using System.Threading.Tasks;
using Domain.Patron;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer.Repositories
{
    public class PatronRepository : Repository<Patron>, IPatronRepository
    {
        public PatronRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void Add(Patron patron) {
            Insert(patron);
        }

        public async Task<Patron> GetById(Guid id)
        {
            return await ById(id);
        }
    }
}