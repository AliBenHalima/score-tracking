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

builder.Services.AddSwaggerGen();

var app = builder.Build();
 
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.MapControllers();
await app.RunAsync();