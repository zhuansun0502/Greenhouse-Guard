using GreenhouseGuard.Api.APIs;
using GreenhouseGuard.Api.DataStore;
using GreenhouseGuard.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<SensorDataStorage>();
builder.Services.AddSingleton<AnomalyDetector>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
GreenhouseAPIs.MapGreenhouseAPIs(app);

app.Run();