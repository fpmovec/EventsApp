using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Infrastructure.ModelsConfiguration
{
    internal class BookingModelConfiguration : IEntityTypeConfiguration<Booking>
    {
        private readonly Expression<Func<DateTime, DateTime>> dateToUTC =
            src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc);

        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(b => b.CreatedDate)
                .HasConversion(dateToUTC, dateToUTC);
        }
    }
}
