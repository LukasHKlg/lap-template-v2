using OnlineShop.Web.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Blazored.LocalStorage;
using OnlineShop.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineShop.Web.ApiClients;
using OnlineShop.Web.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();

//FluentUI
builder.Services.AddFluentUIComponents();
builder.Services.AddDataGridEntityFrameworkAdapter();

builder.Services.AddOutputCache();


//Auth
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        //options.Cookie.Expiration = TimeSpan.FromMinutes(120);
        options.Cookie.Name = "AuthTokenCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
        options.LoginPath = "/login";
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new("https+http://apiservice");
});


builder.Services.AddScoped<WeatherApiClient>();
builder.Services.AddScoped<ProductApiClient>();
builder.Services.AddScoped<CartApiClient>();
builder.Services.AddScoped<CustomerApiClient>();
builder.Services.AddTransient<AppsettingsConfigService>();
builder.Services.AddScoped<SalesOrderApiClient>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();
app.UseOutputCache();

app.UseStaticFiles();
app.MapRazorPages();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map endpoints
app.MapDefaultEndpoints();

app.Run();
