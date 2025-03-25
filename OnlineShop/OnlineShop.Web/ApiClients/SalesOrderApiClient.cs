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

    public SalesOrderApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _httpContextAccessor = httpContextAccessor;
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

            var responseItem = await response.Content.ReadFromJsonAsync<SalesOrderDTO>();

            return responseItem;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> DeleteProductFromCart(CartItemDTO cartItemToDelete, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/CartItems/{cartItemToDelete.Id}";

            var response = await _httpClient.DeleteAsync(requestUrl);

            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> UpdateQuantity(CartItemDTO productToUpdate, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/CartItems/{productToUpdate.Id}";

            var response = await _httpClient.PutAsJsonAsync<CartItemDTO>(requestUrl, productToUpdate);

            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
