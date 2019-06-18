using System;
using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.SQLServer.Mappings
{
    public abstract class MappingBase
    {
        public void ApplyEntityMapping<T>(EntityTypeBuilder<T> builder) where T : Entity
        {
             var converter = new ValueConverter<Guid, string>(
                v => v.ToString(),
                v => new Guid(v));

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(converter)
                .ValueGeneratedNever();

            
        }
    }
}