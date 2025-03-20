using Microsoft.AspNetCore.Mvc;
using OnlineShop.ApiService.Models;
using OnlineShop.Shared.DTOs;

namespace OnlineShop.ApiService.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecastDTO> Get()
    {
        try
        {
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
                    .ToArray();

            return forecasts.Select(x =>
                new WeatherForecastDTO(x.Date, x.TemperatureC, x.TemperatureF, x.Summary));
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while getting WeatherForecasts, Error: {0}", ex);
            throw;
        }
        
    }
}
