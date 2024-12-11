using Arcadia.Shared.Models.WebApp.SysLib;
using Arcadia.WebApp.Components;
using Arcadia.WebApp.Interfaces;
using Arcadia.WebApp.Services;
using Microsoft.Extensions.Options;

namespace Arcadia.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add Blazor.Bootstrap
        builder.Services.AddBlazorBootstrap();
        
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Configure HttpClientFactory with named clients
        builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

        builder.Services.AddHttpClient<ICorporateTravelAssistantService, CorporateTravelAssistantService>((serviceProvider, client) =>
        {
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value.CorporateTravelAssistant;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            // Add other default headers or configurations if needed
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
