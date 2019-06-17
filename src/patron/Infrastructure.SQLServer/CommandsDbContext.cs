using Infrastructure.SQLServer.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer
{
    public class CommandsDbContext : DbContext
    {
        public CommandsDbContext(DbContextOptions<CommandsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.ApplyConfiguration(new PatronMapping());
        }
    }
}