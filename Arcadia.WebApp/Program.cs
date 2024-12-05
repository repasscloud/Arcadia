using Arcadia.WebApp.Components;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;

namespace Arcadia.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 1. Add persistent data protection with encryption using a certificate
        var certificatePath = @"/etc/arcadia/certs/arcadia_webapp_cert.pfx"; // Path to your PFX file
        var certificatePassword = "your-password"; // Password for the PFX file
        if (!File.Exists(certificatePath))
            throw new InvalidOperationException($"Certificate missing: {certificatePath}");
        if (string.IsNullOrEmpty(certificatePassword))
            throw new InvalidOperationException("Password for certificate is not provided.");

        var certificate = new X509Certificate2(certificatePath, certificatePassword);

        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(@"/etc/arcadia/dataprotection-keys")) // Example for Docker
            .ProtectKeysWithCertificate(certificate);

        // 2. Add Blazor.Bootstrap
        builder.Services.AddBlazorBootstrap();
        
        // 3. Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var app = builder.Build();

        // 4. Add Health Check Endpoint
        app.MapGet("/health", () => Results.Ok("OK"));

        // 5. Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();
        app.UseAntiforgery();

        // 6. Map Razor Components
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // 7. Run the application
        app.Run();
    }
}
