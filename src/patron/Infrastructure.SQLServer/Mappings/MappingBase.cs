using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.SQLServer.Mappings
{
    public abstract class MappingBase
    {
        public void ApplyEntityMapping<T>(EntityTypeBuilder<T> builder) where T : Entity
        {
            builder.HasKey(x => x.Id);
        }
    }
}