using Domain.Patron.ValueObjects;

namespace Domain.Patron.Factories
{
    public interface INewPatronData
    {
         string FirstName { get; }
         string LastName { get; }
         EmailAddress Email { get; }
    }
}