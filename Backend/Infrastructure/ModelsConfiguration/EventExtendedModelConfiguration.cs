using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration
{
    public class EventExtendedModelConfiguration : IEntityTypeConfiguration<EventExtendedModel>
    {
        public void Configure(EntityTypeBuilder<EventExtendedModel> builder)
        {
            builder.HasMany<Booking>()
                   .WithOne()
                   .HasForeignKey(t => t.EventId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
