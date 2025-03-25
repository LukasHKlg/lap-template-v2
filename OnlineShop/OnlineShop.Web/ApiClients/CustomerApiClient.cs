using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Shared.DTOs;
using OnlineShop.Shared.Models;
using OnlineShop.Web.Services;

namespace OnlineShop.Web.ApiClients;

public class CustomerApiClient
{
    private HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PaginatedList<CustomerDTO>> GetCustomersAsync(int pageIndex = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/customers?pageIndex={pageIndex}&pageSize={pageSize}";

            var response = await _httpClient.GetFromJsonAsync<PaginatedList<CustomerDTO>>(requestUrl);

            return response;
        }
        catch (Exception ex)
        {
            //TODO what if exception is becaus of unauthorized?
            throw;
        }

    }

    public async Task<PaginatedList<CustomerDTO>> GetCustomersForSearchAsync(string searchValue, int pageIndex = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/customers/search?pageIndex={pageIndex}&pageSize={pageSize}&searchValue={searchValue}";

            var response = await _httpClient.GetFromJsonAsync<PaginatedList<CustomerDTO>>(requestUrl);

            return response;
        }
        catch (Exception ex)
        {
            //TODO what if exception is becaus of unauthorized?
            throw;
        }
    }

    public async Task<CustomerDTO> GetCustomerDetailsAsync(int customerId, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/customers/{customerId}";

            var response = await _httpClient.GetFromJsonAsync<CustomerDTO>(requestUrl);

            return response;
        }
        catch (Exception ex)
        {
            throw;
        }
    }


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

    public async Task<bool> DeleteCustomer(CustomerDTO customerToDelete, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/customers/{customerToDelete.Id}";

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

    public async Task<bool> UpdateCustomer(CustomerDTO customerToUpdate, CancellationToken cancellationToken = default)
    {
        try
        {
            Helpers.Helpers.GetUserAuthToken(_httpContextAccessor.HttpContext, ref _httpClient);

            string requestUrl = $"/api/customers/{customerToUpdate.Id}";

            var response = await _httpClient.PutAsJsonAsync<CustomerDTO>(requestUrl, customerToUpdate);

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
}
