using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration
{
    internal class IdentityUserModelConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            builder.HasMany<Booking>()
                .WithOne()
                .HasForeignKey(u => u.UserId)
                .IsRequired(false);
        }
    }
}
