using Entities.Models;
using Infrastructure.ModelsConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure
{
    public class EventContext : IdentityDbContext
    {
        public EventContext(DbContextOptions<EventContext> opts) : base(opts)
        { }

        private readonly Expression<Func<DateTime, DateTime>> dateToUTC = 
            src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventBaseModel>().ToTable("EventsBase");
            modelBuilder.Entity<EventExtendedModel>().ToTable("ExtendedEvents");

            ConfigureRelations(modelBuilder);
            ApplyModelsConfiguration(modelBuilder);
        }

        private void ApplyModelsConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventModelConfiguration());
        }

        private void ConfigureRelations(ModelBuilder modelBuilder)
        {
            var sourceDate = 

            modelBuilder.Entity<EventExtendedModel>()
                            .HasMany<Booking>()
                            .WithOne()
                            .HasForeignKey(t => t.EventId)
                            .IsRequired()
                            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventBaseModel>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<IdentityUser>()
                .HasMany<Booking>()
                .WithOne()
                .HasForeignKey(u => u.UserId)
                .IsRequired(false);

            modelBuilder.Entity<EventBaseModel>()
                .HasOne(e => e.Image)
                .WithOne()
                .HasForeignKey<Image>(i => i.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventBaseModel>()
                .HasOne(e => e.Category)
                .WithMany();

            modelBuilder.Entity<Booking>()
                .Property(b => b.CreatedDate)
                .HasConversion(dateToUTC, dateToUTC);

            modelBuilder.Entity<EventBaseModel>()
                .Property(e => e.Date)
                .HasConversion(dateToUTC, dateToUTC);

            modelBuilder.Entity<RefreshToken>()
                .Property(r => r.CreatedAt)
                .HasConversion(dateToUTC, dateToUTC);

            modelBuilder.Entity<RefreshToken>()
                .Property(r => r.ExpiryDate)
                .HasConversion(dateToUTC, dateToUTC);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<EventBaseModel> Events { get; set; }
        public DbSet<EventExtendedModel> ExtendedEvents { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Image> Images {  get; set; } 
        public virtual DbSet<EventCategory> Categories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
