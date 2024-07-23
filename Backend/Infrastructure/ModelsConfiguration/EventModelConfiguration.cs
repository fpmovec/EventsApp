using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration
{
    internal class EventModelConfiguration : IEntityTypeConfiguration<EventBaseModel>
    {
        public void Configure(EntityTypeBuilder<EventBaseModel> modelBuilder)
        {
            modelBuilder.Property(e => e.Name)
                 .IsRequired()
                 .HasMaxLength(60);

            modelBuilder.Property(e => e.Description)
                .IsRequired();

            modelBuilder.Property(e => e.Place)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Property(e => e.Price)
                .IsRequired();
        }
    }
}
