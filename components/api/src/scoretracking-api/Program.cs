using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ScoreTracking.App.Services;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ScoreTracking.App.Database;
using Microsoft.Extensions.Configuration;
using ScoreTracking.App.Repositories;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Middlewares;
using FluentValidation.AspNetCore;
using System.Reflection;
using FluentValidation;
using ScoreTracking.App.DTOs.Requests;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ScoreTracking.App.Options.OptionSetup;
using ScoreTracking.App.Providers;
using ScoreTracking.App.Interfaces.Providers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.RateLimiting;
using ScoreTracking.Extensions.Email;
using BenchmarkDotNet.Running;
using ScoreTracking.API.Controllers;
using ScoreTracking.API;
using ScoreTracking.App;
using ScoreTracking.Extensions.Email.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")).UseSnakeCaseNamingConvention());
builder.Services.AddAutoMapper(typeof(IModuleMarker).Assembly);
builder.Services.AddControllers().AddFluentValidation(x =>
{
    x.ImplicitlyValidateChildProperties = true;
    x.RegisterValidatorsFromAssembly(typeof(IModuleMarker).Assembly);
}).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//Reposirtories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IRoundRepository, RoundRepository>();



//Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IRoundService, RoundService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddMailing();
builder.Services.AddLogging();



//BenchmarkRunner.Run<TestClass>();

//Hosted Services


builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    rateLimiterOptions.AddPolicy("sliding-signin", httpContext =>
        RateLimitPartition.GetSlidingWindowLimiter(partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromSeconds(60),
                SegmentsPerWindow = 10

            }) 
        ); 
    });

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
 {
     {
           new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
             },
             new string[] {}
     }
 });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Score Trackig API",
        Version = "v1",
        Description = "Score Trackig Web API Documetatio",
        Contact = new OpenApiContact
        {
            Name = "Contact",
            Email = "score-trackig@gmail.com",
            Url = new Uri("https://example.com/contact"),
        },
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionSetup>();
builder.Services.ConfigureOptions<MailSettingsSetup>();


var app = builder.Build();
 
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});
app.UseAuthorization();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();
await app.RunAsync();