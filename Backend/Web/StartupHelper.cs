using Application.CollectionServices;
using Application.CollectionServices.Filter;
using Application.CollectionServices.Sort;
using Entities.Models;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Web
{
    internal static class StartupHelper
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opts =>
             {
                opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opts.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                   {
                      new OpenApiSecurityScheme
                      {
                          Reference = new OpenApiReference
                          {
                             Type=ReferenceType.SecurityScheme,
                             Id="Bearer"
                          }
                      },
                      new string[]{}
                   }
                });
             });

            return services;
        }

        public static IServiceCollection AddModelsFilters(this IServiceCollection services)
        {
            services.AddSingleton<IFilterService<EventBaseModel>, EventsFilterService>();
            services.AddSingleton<IFilterService<EventCategory>, CategoriesFilterService>();
            services.AddSingleton<IFilterService<Booking>, BookingFilterService>();

            return services;
        }

        public static IServiceCollection AddModelsSort(this IServiceCollection services)
        {
            services.AddSingleton<ISortService<EventBaseModel>, EventsSortService>();
            services.AddSingleton<ISortService<EventCategory>, CategoriesSortService>();
            services.AddSingleton<ISortService<Booking>, BookingSortService>();

            return services;
        }

        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using var serviceScope = app.Services
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            using var context = serviceScope.ServiceProvider
                .GetRequiredService<EventContext>();

            context.Database.Migrate();

            return app;
        }
    }
}
