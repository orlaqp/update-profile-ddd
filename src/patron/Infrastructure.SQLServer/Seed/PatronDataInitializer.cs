using System;
using System.Collections.ObjectModel;
using Domain.Patron;
using Domain.Patron.Factories;
using DTOs.Patron;
using Infrastructure.SQLServer.Repositories;

namespace Infrastructure.SQLServer.Seed
{
    public static class PatronDataInitializer
    {
        public static void Initialize(CommandsDbContext context, IServiceProvider services) {
            context.Database.EnsureCreated();
            var patronRepo = services.GetService(typeof(PatronRepository)) as PatronRepository;

            var patrons = new Collection<Patron>();
            patrons.Add(PatronFactory.CreateFrom(new NewPatronData { FirstName = "First", LastName = "Last", Email = new Domain.Patron.ValueObjects.EmailAddress("email@address.com") }));

            foreach (var patron in patrons)
            {
                patronRepo.Add(patron);
            }

            context.SaveChanges();
        }
    }
}