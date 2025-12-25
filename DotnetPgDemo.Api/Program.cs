using DotnetPgDemo.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// registering our AppDB context in the DI container
// It is registered as a scoped lifetime
string connectionString = builder.Configuration.GetConnectionString("Default") ?? 
throw new InvalidOperationException("Connection string with name 'Default' does not exist");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

app.MapControllers();

app.Run();
