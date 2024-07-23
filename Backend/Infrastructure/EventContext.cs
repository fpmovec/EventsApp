using Entities.Models;
using Infrastructure.ModelsConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata;

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

            ApplyModelsConfiguration(modelBuilder);
            ConfigureRelations(modelBuilder);
        }

        private void ApplyModelsConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventModelConfiguration());
            modelBuilder.ApplyConfiguration(new EventExtendedModelConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenModelConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityUserModelConfiguration());
            modelBuilder.ApplyConfiguration(new BookingModelConfiguration());
        }

        private void ConfigureRelations(ModelBuilder modelBuilder)
        {
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
