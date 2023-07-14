using Common.WebApi.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ScoreTracking.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//OptionsBuilder.UseNpgsql(@"Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
