using Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class SeedData
    {
        public static void PopulateDatabase(this IApplicationBuilder app)
        {
            using  var scope = app.ApplicationServices.CreateScope();
            EventContext context = scope.ServiceProvider.GetRequiredService<EventContext>();
            AddInitialData(context);
        }

        private static void AddInitialData(EventContext context)
        {
            if (!context.Categories.Any())
            {
                List<EventCategory> categories = [
                    new EventCategory { Name = "Festival" },
                    new EventCategory { Name = "Concert" },
                    new EventCategory { Name = "Conference" }];

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            if (context.Events.Any())
                return;

            EventCategory category = context.Categories.First();

            List<EventExtendedModel> events =
            [
                new()
                {
                    Name = "Modsen event",
                    Description = "Modsen event description",
                    Price = 5,
                    Date = DateTime.Now,
                    Place = "Minsk",
                    Category = category,
                    MaxParticipantsCount = 5,
                    CategoryId = category.Id,
                    Image = new()
                    {
                        Name = "no-image",
                        Path = Path.Combine("images", "no-image.jpg")
                    }
                },
                new()
                {
                    Name = "Modsen event 2",
                    Description = "Modsen event description 2",
                    Price = 15,
                    Date = DateTime.Now.AddDays(5),
                    Place = "Moscow",
                    Category = category,
                    MaxParticipantsCount = 15,
                    CategoryId = category.Id,
                    Image = new()
                    {
                        Name = "no-image",
                        Path = Path.Combine("images", "no-image.jpg")
                    }
                }
            ];

            context.ExtendedEvents.AddRange(events);
            context.SaveChanges();
        }
    }
}
