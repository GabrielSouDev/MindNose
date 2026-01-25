using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MindNose.Domain.Configurations;
using MindNose.Infrastructure.Persistence;
using System.Text;

namespace MindNose.Apresentation.Extensions;

public static class AuthExtensions
{
    public static void AddAuth(this WebApplicationBuilder builder)
    {
        // TODO: Ajustar Identity e JWT para subir para PROD.
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
        })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

        var jwtSettings = builder.Configuration.GetSection("JWT").Get<JWTSettings>() ?? throw new Exception("Não foi possivel configurar JWT.");

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateIssuer = !string.IsNullOrWhiteSpace(jwtSettings.Issuer),
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = !string.IsNullOrWhiteSpace(jwtSettings.Audience),
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(int.Parse(jwtSettings.ExpireMinutes))
                    };
                });
    }
}