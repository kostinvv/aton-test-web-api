using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Models;
using WebAPI.Options;

namespace WebAPI.ApiExtensions;

public static class AuthExtension
{
    public static void AddAuthConfiguration(this IServiceCollection service, IConfiguration config)
    {
        service.Configure<JwtOptions>(config.GetSection(nameof(JwtOptions)));
        
        service.AddAuthentication(configureOptions: options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                var jwtOptions = config.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
                
                options.Audience = jwtOptions!.Audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

        service.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", builder 
                => builder.RequireClaim(ClaimTypes.Role, Role.Admin.ToString()));
    }
}