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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllers().AddFluentValidation(x =>
{
    x.ImplicitlyValidateChildProperties = true;
    x.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()); // TODO : Normally we need to specify the exact Assembly (not all assemblies) 
});

builder.Services.AddSwaggerGen();

var app = builder.Build();
 
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.MapControllers();
await app.RunAsync();