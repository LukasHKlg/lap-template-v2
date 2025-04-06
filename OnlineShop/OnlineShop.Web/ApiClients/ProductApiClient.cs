using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Shared.DTOs;
using OnlineShop.Shared.Models;
using OnlineShop.Web.Services;

namespace OnlineShop.Web.ApiClients;

public class ProductApiClient
{
    private HttpClient _httpClient;
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

    public async Task<ProductDTO> GetProductDetailsAsync(int productId, CancellationToken cancellationToken = default)
    {
        try
        {
            string requestUrl = $"/api/products/{productId}";

            var response = await _httpClient.GetFromJsonAsync<ProductDTO>(requestUrl);

            return response;
        }
        catch (Exception ex)
        {
            throw;
        }

    }


    #region ProductAdmin
    public async Task<ProductDTO> AddNewProductAsync(ProductDTO newProduct, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/products";

            var response = await _httpClient.PostAsJsonAsync<ProductDTO>(requestUrl, newProduct);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException("Unauthorized Access: " + response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<ProductDTO>();

            return result;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    public async Task<UploadPictureResponse> UploadProductPicture(MultipartFormDataContent pictureContent, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/files/uploadProductPicture";

            var response = await _httpClient.PostAsync(requestUrl, pictureContent);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException("Unauthorized Access: " + response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<UploadPictureResponse>();

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> DeleteProduct(ProductDTO productToDelete, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/products/{productToDelete.Id}";

            var response = await _httpClient.DeleteAsync(requestUrl);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException("Unauthorized Access: " + response.StatusCode);

            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> DeleteProductPicture(ProductDTO productToDelete, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/files/{productToDelete.Id}";

            var response = await _httpClient.DeleteAsync(requestUrl);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException("Unauthorized Access: " + response.StatusCode);

            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> UpdateProduct(ProductDTO productToUpdate, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/products/{productToUpdate.Id}";

            var response = await _httpClient.PutAsJsonAsync<ProductDTO>(requestUrl, productToUpdate, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException("Unauthorized Access: " + response.StatusCode);

            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public class UploadPictureResponse
    {
        public string FileUrl { get; set; }
    }
    #endregion ProductAdmin
}
