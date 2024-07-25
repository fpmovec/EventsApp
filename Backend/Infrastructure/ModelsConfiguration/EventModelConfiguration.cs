using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Infrastructure.ModelsConfiguration
{
    internal class EventModelConfiguration : IEntityTypeConfiguration<EventBaseModel>
    {
        private readonly Expression<Func<DateTime, DateTime>> dateToUTC =
           src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc);
        public void Configure(EntityTypeBuilder<EventBaseModel> modelBuilder)
        {
            modelBuilder.HasKey(e => e.Id);

            modelBuilder.HasOne(e => e.Image)
                .WithOne()
                .HasForeignKey<Image>(i => i.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.HasOne(e => e.Category)
                .WithMany();

            modelBuilder.Property(e => e.Date)
                .HasConversion(dateToUTC, dateToUTC);

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
