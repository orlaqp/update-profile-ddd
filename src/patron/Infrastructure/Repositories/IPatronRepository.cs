using System;
using System.Threading.Tasks;
using Domain.Patron;

namespace Infrastructure.Repositories
{
    public interface IPatronRepository
    {
         void Add(Patron patron);
         Task<Patron> GetById(Guid id);
    }
}