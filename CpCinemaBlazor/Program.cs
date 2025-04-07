using Blazored.LocalStorage;
using CpCinemaBlazor;
using CpCinemaBlazor.ApiRequest;
using CpCinemaBlazor.ApiRequest.Services;
using CpCinemaBlazor.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
    });
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<FilmService>();
builder.Services.AddScoped<MessagesService>();

builder.Services.AddHttpClient("UnauthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5005");
});
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5005");
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();
builder.Services.AddTransient<AuthorizationMessageHandler>();
builder.Services.AddAuthorizationCore();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAntiforgery();



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
