using Domain.Patron.Factories;
using Domain.Patron.ValueObjects;

namespace DTOs.Patron
{
    public class NewPatronData : INewPatronData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public EmailAddress Email { get; set; }
    }
}