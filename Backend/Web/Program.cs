using Application.CollectionServices;
using Domain.AppSettings;
using Domain.Models;
using Infrastructure;
using Infrastructure.CollectionServices;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Web;
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();

builder.Services.AddOptions<AppSettings>()
    .BindConfiguration(nameof(AppSettings))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<ISortService<EventBaseModel>, EventsSortService>();
builder.Services.AddSingleton<IFilterService<EventBaseModel>, EventsFilterService>();

builder.Services
    .AddDbContext<EventContext>(opts =>
    {
        opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = responseContext =>
    {
        ResponseHeaders headers = responseContext.Context.Response.GetTypedHeaders();
        headers.CacheControl = new CacheControlHeaderValue
        {
            Public = true,
            MaxAge = TimeSpan.FromDays(30)
        };
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandlingMiddleware();
app.UseRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
