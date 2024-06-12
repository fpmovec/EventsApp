using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class EventContext : DbContext
    {
        public EventContext(DbContextOptions<EventContext> opts) : base(opts)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventBaseModel>().ToTable("EventsBase");
            modelBuilder.Entity<EventExtendedModel>().ToTable("ExtendedEvents");

            ConfigureRelations(modelBuilder);
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
                .IsRequired();

            modelBuilder.Entity<EventBaseModel>()
                .HasOne(e => e.Category)
                .WithOne();
        }

        public DbSet<EventBaseModel> Events { get; set; }
        public DbSet<EventExtendedModel> ExtendedEvents { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images {  get; set; } 
        public DbSet<Participant> Participants { get; set; }
        public DbSet<EventCategory> Categories { get; set; }
    }
}
