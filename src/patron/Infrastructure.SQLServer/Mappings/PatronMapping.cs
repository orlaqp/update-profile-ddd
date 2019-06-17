using Domain.Patron;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.SQLServer.Mappings
{
    public class PatronMapping : MappingBase, IEntityTypeConfiguration<Patron>
    {
        public void Configure(EntityTypeBuilder<Patron> builder)
        {
            base.ApplyEntityMapping(builder);
            builder.OwnsOne(
                x => x.EmailAddress,
                a => a.Property(p => p.Email).IsRequired().HasColumnName("emailAddress")
            );
        }
    }
}