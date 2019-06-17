using Domain.Patron.ValueObjects;

namespace Domain.Patron.Factories
{
    public static class PatronFactory
    {
        public static Patron CreateFrom(INewPatronData data) {
            return new Patron(data);
        }
    }
}