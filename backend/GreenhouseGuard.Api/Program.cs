using GreenhouseGuard.Api.APIs;
using GreenhouseGuard.Api.DataStore;
using GreenhouseGuard.Api.Hubs;
using GreenhouseGuard.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<SensorDataStorage>();
builder.Services.AddSingleton<AnomalyDetector>();

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy => policy
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");

GreenhouseAPIs.MapGreenhouseAPIs(app);
app.MapHub<GreenhouseHub>("live");

app.Run();