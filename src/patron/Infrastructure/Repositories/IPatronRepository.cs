using System;
using Domain.Patron;

namespace Infrastructure.Repositories
{
    public interface IPatronRepository
    {
         Patron GetById(Guid id);
    }
}