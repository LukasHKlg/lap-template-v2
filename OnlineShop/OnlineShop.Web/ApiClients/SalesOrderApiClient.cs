using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Shared.DTOs;
using OnlineShop.Shared.Models;
using OnlineShop.Web.Services;

namespace OnlineShop.Web.ApiClients;

public class SalesOrderApiClient
{
    private HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SalesOrderApiClient> _logger;

    public SalesOrderApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor , ILogger<SalesOrderApiClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<string> GetOrderInvoiceAsync(SalesOrderDTO salesOrder, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/salesorders/getinvoice/{salesOrder.Id}";

            var response = await _httpClient.GetAsync(requestUrl);
            var bytes = await response.Content.ReadAsByteArrayAsync();

            var base64 = Convert.ToBase64String(bytes);
            var url = $"data:application/pdf;base64, {base64}";

            return url;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<SalesOrderDTO> AddOrderAsync(SalesOrderDTO newOrder, CancellationToken cancellationToken = default)
    {
        try
        {
           Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/salesorders";

            var response = await _httpClient.PostAsJsonAsync<SalesOrderDTO>(requestUrl, newOrder, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseItem = await response.Content.ReadFromJsonAsync<SalesOrderDTO>();

                return responseItem;
            }
            else
            {
                var responseMsg = await response.Content.ReadAsStringAsync();

                return null;
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<List<SalesOrderDTO>> GetSalesOrdersForCustomer(CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/salesorders/ordersuser";

            var response = await _httpClient.GetFromJsonAsync<List<SalesOrderDTO>>(requestUrl, cancellationToken);

            if (response != null)
            {
                return response;
            }
            else return null;
        }
        catch (Exception)
        {

            throw;
        }
    }
    
    public async Task<PaginatedList<SalesOrderDTO>> GetPaginatedSalesOrders(int pageIndex = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/salesorders?pageIndex={pageIndex}&pageSize={pageSize}";

            var response = await _httpClient.GetFromJsonAsync<PaginatedList<SalesOrderDTO>>(requestUrl, cancellationToken);

            if (response != null)
            {
                return response;
            }
            else return null;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> UpadateShippedDate(SalesOrderDTO orderToUpdate, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/salesorders/{orderToUpdate.Id}";

            var response = await _httpClient.PutAsJsonAsync<SalesOrderDTO>(requestUrl, orderToUpdate, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return response.IsSuccessStatusCode;
            }
            else
            {
                var responseMsg = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error updating shipped date. Response Msg: " + responseMsg);
                return response.IsSuccessStatusCode;
            }
        }
        catch (Exception)
        {
            
            throw;
        }
    }
}
