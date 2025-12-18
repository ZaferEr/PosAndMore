using PosAndMore.API;
using Microsoft.Data.SqlClient;
using Dapper;
using PosAndMore.Models;  // Kdv sınıfın burada olsun

var connectionString = "Server=89.144.20.242;Database=EuroDigiPos;User Id=sa;Password=mb88421;TrustServerCertificate=True;";
var builder = WebApplication.CreateBuilder(args);
// Controller'ları ekle
builder.Services.AddControllers();  // Bu satır önemli!

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// Controller route'larını map'le
app.MapControllers();  // Bu satırla tüm controller'lar otomatik keşfedilir

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{

 

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
