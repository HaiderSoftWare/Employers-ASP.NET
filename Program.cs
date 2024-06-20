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
builder.Services.AddSingleton<List<EmploerModel>>(); // Register a singleton list to hold employers.
builder.Services.AddDbContext<EmployerDb>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// Function to check database connection
bool IsConnectionWorking(string connectionString)
{
    try
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine($"The connection is successful.");
            return true;
        }
    }
    catch (Exception ex)
    {
        // Log exception details here if necessary
        Console.WriteLine($"Connection failed: {ex.Message}");
        return false;
    }
}

// Check the database connection
var connectionString = configuration.GetConnectionString("DefaultConnection");
if (!IsConnectionWorking(connectionString!))
{
    throw new Exception("Database connection failed. Please check the connection string and database server.");
}

var app = builder.Build();
app.UseHttpsRedirection();
app.MapEmployerEndpoints(app.Services.GetRequiredService<List<EmploerModel>>());



app.Run();
