using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration
{
    internal class EventModelConfiguration : IEntityTypeConfiguration<EventExtendedModel>
    {
        public void Configure(EntityTypeBuilder<EventExtendedModel> modelBuilder)
        {
            
        }
    }
}
