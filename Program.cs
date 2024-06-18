using employers.Endpoints;
using employers.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<List<EmploerModel>>(); // Register a singleton list to hold employers.

var app = builder.Build();
app.UseHttpsRedirection();
app.MapEmployerEndpoints(app.Services.GetRequiredService<List<EmploerModel>>());
app.Run();
