using Arcadia.API.Data;
using Arcadia.API.Interfaces;
using Arcadia.API.Services;
using Arcadia.Shared.Models.SysLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Arcadia.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Configure Configuration to Load from appsettings.json and Environment Variables
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables(); // Add environment variables to the configuration

            // 2. Load and validate JwtSettings from environment or fallback to appsettings.json
            var jwtSettings = new JwtSettings
            {
                Secret = builder.Configuration["JwtSettings:Secret"]
                        ?? builder.Configuration.GetValue<string>("JwtSettings:Secret")
                        ?? throw new InvalidOperationException("JWT Secret is not configured."),

                Issuer = builder.Configuration["JwtSettings:Issuer"]
                        ?? builder.Configuration.GetValue<string>("JwtSettings:Issuer")
                        ?? throw new InvalidOperationException("JWT Issuer is not configured."),

                Audience = builder.Configuration["JwtSettings:Audience"]
                        ?? builder.Configuration.GetValue<string>("JwtSettings:Audience")
                        ?? throw new InvalidOperationException("JWT Audience is not configured.")
            };


            if (string.IsNullOrEmpty(jwtSettings.Secret) ||
                string.IsNullOrEmpty(jwtSettings.Issuer) ||
                string.IsNullOrEmpty(jwtSettings.Audience))
            {
                throw new InvalidOperationException("JWT settings are not properly configured in environment variables or appsettings.json.");
            }

            builder.Services.AddSingleton(jwtSettings); // Register JwtSettings as a singleton

            // 3. Load and validate the connection string from environment or fallback to appsettings.json
            var connectionString = builder.Configuration["CONNECTION_STRING"] 
                                   ?? builder.Configuration.GetConnectionString("DefaultConnection")
                                   ?? string.Empty;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is not configured in environment variables or appsettings.json.");
            }

            // 4. Register the DbContext with PostgreSQL provider
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString)
            );

            // 5. Add Identity services
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // 6. Add Email Sender Service
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            // 7. Register and validate JwtSettings
            builder.Services.AddSingleton<IValidateOptions<JwtSettings>, JwtSettingsValidation>();

            // 8. Configure JWT authentication
            var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // 9. Add controllers
            builder.Services.AddControllers();

            // 10. Register Swagger services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 11. Build the app (only once)
            var app = builder.Build();

            // 12. Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication(); // Ensure authentication middleware is added before authorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        // Custom validation class
        public class JwtSettingsValidation : IValidateOptions<JwtSettings>
        {
            public ValidateOptionsResult Validate(string? name, JwtSettings options)
            {
                // Ensure options is not null (defensive programming)
                if (options == null)
                {
                    throw new ArgumentNullException(nameof(options), "JWT settings cannot be null.");
                }

                // Validate required fields
                if (string.IsNullOrEmpty(options.Secret))
                {
                    return ValidateOptionsResult.Fail("JWT Secret is not configured.");
                }

                if (string.IsNullOrEmpty(options.Issuer))
                {
                    return ValidateOptionsResult.Fail("JWT Issuer is not configured.");
                }

                if (string.IsNullOrEmpty(options.Audience))
                {
                    return ValidateOptionsResult.Fail("JWT Audience is not configured.");
                }

                // Return success if all validations pass
                return ValidateOptionsResult.Success;
            }
        }
    }
}
