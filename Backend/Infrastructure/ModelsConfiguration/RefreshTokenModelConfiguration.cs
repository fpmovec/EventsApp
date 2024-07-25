using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Infrastructure.ModelsConfiguration
{
    internal class RefreshTokenModelConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        private readonly Expression<Func<DateTime, DateTime>> dateToUTC =
            src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc);

        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Property(r => r.CreatedAt)
                .HasConversion(dateToUTC, dateToUTC);

            builder.Property(r => r.ExpiryDate)
                .HasConversion(dateToUTC, dateToUTC);
        }
    }
}
