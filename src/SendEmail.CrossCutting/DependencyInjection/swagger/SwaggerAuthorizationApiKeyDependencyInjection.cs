using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace SendEmail.CrossCutting.DependencyInjection.swagger;

public static class SwaggerAuthorizationApiKeyDependencyInjection
{
    public static void AddSwaggerAuthorizationApiKeyDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
            {
                Name = "x-gateway-app",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = @"API KEY Authorization header."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                        new string[] {}
                    }
                });
        });
    }

}
