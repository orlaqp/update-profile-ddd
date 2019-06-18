using System;
using System.Threading.Tasks;
using Domain.Patron;

namespace Infrastructure.Repositories
{
    public interface IPatronRepository
    {
         Task<Patron> GetById(Guid id);
    }
}