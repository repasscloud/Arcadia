using Arcadia.Shared.Models.WebApp.SysLib;
using Arcadia.WebApp.Components;
using Arcadia.WebApp.Interfaces;
using Arcadia.WebApp.Services;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Arcadia.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add Blazor.Bootstrap
            builder.Services.AddBlazorBootstrap();
            
            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Configure ApiSettings and validate
            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

            var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();

            if (string.IsNullOrEmpty(apiSettings?.CorporateTravelAssistant.BaseUrl))
            {
                throw new InvalidOperationException("CorporateTravelAssistant API BaseUrl is not configured.");
            }

            // Register HttpClient with CorporateTravelAssistantService and Polly
            builder.Services.AddHttpClient<ICorporateTravelAssistantService, CorporateTravelAssistantService>((serviceProvider, client) =>
            {
                var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value.CorporateTravelAssistant;
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                // Add other default headers or configurations if needed
            })
            .AddPolicyHandler(GetRetryPolicy()); // Adding Polly for resilience

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

        // Define Polly Retry Policy
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
