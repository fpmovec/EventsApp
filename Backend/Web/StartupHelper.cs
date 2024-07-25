using Application.CollectionServices;
using Application.CollectionServices.Filter;
using Application.CollectionServices.Sort;
using Application.FluentValidation;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Domain.Repositories;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Web.DTO;
using Web.Hubs;

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
            services.AddSingleton<IFilterService<RefreshToken>, TokensFilterService>();

            return services;
        }

        public static IServiceCollection AddModelsSort(this IServiceCollection services)
        {
            services.AddSingleton<ISortService<EventBaseModel>, EventsSortService>();
            services.AddSingleton<ISortService<EventCategory>, CategoriesSortService>();
            services.AddSingleton<ISortService<Booking>, BookingSortService>();
            services.AddSingleton<ISortService<RefreshToken>, TokensSortService>();

            return services;
        }

        public static IServiceCollection AddDtoValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<EventDTO>, EventDTOValidator>();
            services.AddScoped<IValidator<BookingDTO>, BookingDTOValidator>();
            services.AddScoped<IValidator<TokenRequest>, TokenRequestValidator>();
            services.AddScoped<IValidator<LoginDTO>, LoginDTOValidator>();
            services.AddScoped<IValidator<RegisterDTO>, RegisterDTOValidator>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INotificationService, NotificationService<NotificationsHub>>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEventsRepository, EventsBaseRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IJwtRepository, JwtRepository>();

            return services;
        }

        public static IServiceCollection ConfigureAuth(this IServiceCollection services, byte[] key)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(sp => tokenValidationParameters);

            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOpts =>
            {
                jwtOpts.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddDefaultIdentity<IdentityUser>(opts =>
            {
                opts.SignIn.RequireConfirmedEmail = false;
                opts.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<EventContext>();

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
