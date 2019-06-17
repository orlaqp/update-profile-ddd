using Domain.Patron.Factories;

namespace Infrastructure.SQLServer.Seed
{
    public static class PatronDataInitializer
    {
        public static void Initialize(CommandsDbContext context) {
            context.Database.EnsureCreated();
        }
    }
}