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

            // 1. Bind and configure JwtSettings
            var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<JwtSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

            // 2. Validate JwtSettings
            if (jwtSettings == null ||
                string.IsNullOrEmpty(jwtSettings.Secret) ||
                string.IsNullOrEmpty(jwtSettings.Issuer) ||
                string.IsNullOrEmpty(jwtSettings.Audience))
            {
                throw new InvalidOperationException("JWT Settings are not properly configured in appsettings.json.");
            }

            // 3. Register the DbContext with PostgreSQL provider
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // 4. Add Identity services
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // 5. Add Email Sender Service
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            // 6. Register and validate JwtSettings
            builder.Services.AddSingleton<IValidateOptions<JwtSettings>, JwtSettingsValidation>();

            // 7. Configure JWT authentication
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

            // 8. Add controllers
            builder.Services.AddControllers();

            // 9. Register Swagger services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 10. Build the app (only once)
            var app = builder.Build();

            // 11. Configure the HTTP request pipeline.
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
