using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SendEmail.CrossCutting.DependencyInjection;

public static class AuthenticationJwtExtension
{
    public static void AddAuthenticationJwtExtension(this WebApplicationBuilder builder)
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY"));
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = false;
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = Environment.GetEnvironmentVariable("Audience"),
                ValidIssuer = Environment.GetEnvironmentVariable("Issuer"),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
    }

}
