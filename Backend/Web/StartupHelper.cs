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
    }
}
