using Domain.Models;
using Infrastructure.ModelsConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class EventContext : IdentityDbContext
    {
        public EventContext(DbContextOptions<EventContext> opts) : base(opts)
        { }

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
            modelBuilder.Entity<EventExtendedModel>()
                            .HasMany<Ticket>()
                            .WithOne()
                            .HasForeignKey(t => t.EventId)
                            .IsRequired();

            modelBuilder.Entity<Participant>()
                .HasMany<Ticket>()
                .WithOne()
                .HasForeignKey(t => t.ParticipantId)
                .IsRequired();

            modelBuilder.Entity<EventBaseModel>()
                .HasOne(e => e.Image)
                .WithOne()
                .HasForeignKey<Image>(i => i.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventBaseModel>()
                .HasOne(e => e.Category)
                .WithMany();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<EventBaseModel> Events { get; set; }
        public DbSet<EventExtendedModel> ExtendedEvents { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Image> Images {  get; set; } 
        public DbSet<Participant> Participants { get; set; }
        public DbSet<EventCategory> Categories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
