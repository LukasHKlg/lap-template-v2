using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using OnlineShop.Shared.DTOs;
using OnlineShop.Web.Services;

namespace OnlineShop.Web.ApiClients;

public class WeatherApiClient
{
    private readonly HttpClient _httpClient;
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
            // Retrieve the JWT token from the current user's claims.
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                throw new Exception("No access token found.");
            }

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
