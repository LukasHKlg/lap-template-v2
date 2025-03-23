using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Shared.DTOs;
using OnlineShop.Shared.Models;
using OnlineShop.Web.Services;

namespace OnlineShop.Web.ApiClients;

public class ProductApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PaginatedList<ProductDTO>> GetProductsAsync(int pageIndex = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        try
        {
            PaginatedList<ProductDTO>? products = null;

            string requestUrl = $"/api/products?pageIndex={pageIndex}&pageSize={pageSize}";

            var response = await _httpClient.GetFromJsonAsync<PaginatedList<ProductDTO>>(requestUrl);

            return response;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    public async Task<PaginatedList<ProductDTO>> GetProductsForSearchAsync(string searchValue, int pageIndex = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        try
        {
            PaginatedList<ProductDTO>? products = null;

            string requestUrl = $"/api/products/search?pageIndex={pageIndex}&pageSize={pageSize}&searchValue={searchValue}";

            var response = await _httpClient.GetFromJsonAsync<PaginatedList<ProductDTO>>(requestUrl);

            return response;
        }
        catch (Exception ex)
        {
            throw;
        }

    }
}
