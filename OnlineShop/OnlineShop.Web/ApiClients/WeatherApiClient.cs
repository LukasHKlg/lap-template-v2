using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using OnlineShop.Shared.DTOs;
using OnlineShop.Web.Services;

namespace OnlineShop.Web.ApiClients;

public class WeatherApiClient
{
    private HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WeatherApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<WeatherForecastDTO[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            List<WeatherForecastDTO>? forecasts = null;

            await foreach (var forecast in _httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecastDTO>("/api/WeatherForecast/weatherforecast", cancellationToken))
            {
                if (forecasts?.Count >= maxItems)
                {
                    break;
                }
                if (forecast is not null)
                {
                    forecasts ??= [];
                    forecasts.Add(forecast);
                }
            }

            return forecasts?.ToArray() ?? [];
        }
        catch (Exception ex)
        {
            throw;
        }

    }
}
