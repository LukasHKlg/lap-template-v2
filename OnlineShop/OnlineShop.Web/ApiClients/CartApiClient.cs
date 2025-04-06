using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Shared.DTOs;
using OnlineShop.Shared.Models;
using OnlineShop.Web.Services;

namespace OnlineShop.Web.ApiClients;

public class CartApiClient
{
    private HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CartDTO> GetCartDetailsAsync(CancellationToken cancellationToken = default)
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

            string requestUrl = $"/api/carts/getcartforuser";

            var response = await _httpClient.GetFromJsonAsync<CartDTO>(requestUrl);

            return response;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    public async Task<bool> AddProductToCart(ProductDTO productToAdd, CancellationToken cancellationToken = default)
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

            string requestUrl = $"/api/CartItems/{productToAdd.Id}";

            var response = await _httpClient.GetFromJsonAsync<CartItemDTO>(requestUrl);

            //var responseItem = await response.Content.ReadFromJsonAsync<CartItemDTO>();


            return response != null;
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
