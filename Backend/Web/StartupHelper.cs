using Application.CollectionServices;
using Domain.Models;
using Infrastructure.CollectionServices.Filter;
using Infrastructure.CollectionServices.Sort;
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
            services.AddSingleton<IFilterService<Participant>, ParticipantsFilterService>();

            return services;
        }

        public static IServiceCollection AddModelsSort(this IServiceCollection services)
        {
            services.AddSingleton<ISortService<EventBaseModel>, EventsSortService>();
            services.AddSingleton<ISortService<EventCategory>, CategoriesSortService>();
            services.AddSingleton<ISortService<Booking>, BookingSortService>();
            services.AddSingleton<ISortService<Participant>, ParticipantsSortService>();

            return services;
        }
    }
}
