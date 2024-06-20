using employers.Endpoints;
using employers.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Configure services
builder.Services.AddSingleton<List<EmploerModel>>();
builder.Services.AddDbContext<EmployerDb>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();
app.UseHttpsRedirection();
app.MapEmployerEndpoints(app.Services.GetRequiredService<List<EmploerModel>>());





app.Run();
