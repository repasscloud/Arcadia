using Arcadia.WebApp.Components;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;

namespace Arcadia.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 2. Add persistent data protection with encryption using a certificate
        var certificatePath = "/app/dataprotection-keys/arcadia_cert.pfx"; // Path to your PFX file
        var certificatePassword = "your-password"; // Password for the PFX file
        if (!File.Exists(certificatePath))
            throw new InvalidOperationException($"Certificate missing: {certificatePath}");
        if (string.IsNullOrEmpty(certificatePassword))
            throw new InvalidOperationException("Password for certificate is not provided.");

        var certificate = new X509Certificate2(certificatePath, certificatePassword);

        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(@"/app/dataprotection-keys")) // Example for Docker
            .ProtectKeysWithCertificate(certificate);

        // Add Blazor.Bootstrap
        builder.Services.AddBlazorBootstrap();
        
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

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
