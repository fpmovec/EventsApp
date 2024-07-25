using Domain.AppSettings;
using Infrastructure;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;
using System.Text;
using Web;
using Web.Background;
using Web.Extensions;
using Microsoft.EntityFrameworkCore;
using Domain.UnitOfWork;
using Web.Hubs;
using Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();

builder.Services.AddOptions<AppSettings>()
    .BindConfiguration(nameof(AppSettings))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddModelsFilters();
builder.Services.AddModelsSort();

builder.Services.AddScoped<FilterOptionsConverter>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddDtoValidators();

builder.Services.AddHostedService<BackgroundWorker>();

builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.InstanceName = "local";
    opts.Configuration = "localhost:6379";
});

builder.Services
    .AddDbContext<EventContext>(opts =>
    {
        opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

string secret = builder.Configuration.GetSection($"{nameof(AppSettings)}:{nameof(JwtSettings)}:SecretKey").Value;
byte[] key = Encoding.ASCII.GetBytes(secret);

builder.Services.ConfigureAuth(key);

builder.Services.AddSignalR(opts =>
{
    opts.EnableDetailedErrors = true;
});

builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(build =>
    {
        build.AllowAnyHeader();
        build.AllowAnyMethod();
        build.WithOrigins("http://localhost:5173");
        build.AllowCredentials();
    });
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

app.MigrateDatabase();
app.PopulateDatabase();

app.UseImagesCaching();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors();

app.MapHub<NotificationsHub>("api/notification");
app.UseExceptionHandlingMiddleware();
app.UseRequestLogging();


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
