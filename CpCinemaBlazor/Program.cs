using CpCinemaBlazor.ApiRequest;
using CpCinemaBlazor.ApiRequest.Services;
using CpCinemaBlazor.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ApiRequestService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5005/") });

builder.Services.AddSingleton<UserService>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5005/");
});
//.AddHttpMessageHandler<BearerTokenHandler>();

//// Регистрация BearerTokenHandler
//builder.Services.AddTransient<BearerTokenHandler>(sp =>
//{
//    var localStorage = sp.GetRequiredService<LocalStorageService>();
//    return new BearerTokenHandler(localStorage);
//});

builder.Services.AddSingleton<UserService>();

builder.Services.AddAuthorization();

var app = builder.Build();

// Загрузка токена при старте приложения
//using (var scope = app.Services.CreateScope())
//{
//    var localStorage = scope.ServiceProvider.GetRequiredService<LocalStorageService>();
//    SingletoneUser.Init(localStorage);
//    await SingletoneUser.LoadUserFromLocalStorage();
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorPages();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
