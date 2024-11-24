using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SendEmail.Application.Validators;
using SendEmail.CrossCutting.DependencyInjection.swagger;
using SendEmail.Domain.DTOs.Request;
using SendEmail.Domain.Interfaces.Repositories;
using SendEmail.Domain.Interfaces.Services;
using SendEmail.Domain.Services;
using SendEmail.Infra.Data.Data;
using SendEmail.Infra.Data.Repositories;

namespace SendEmail.CrossCutting.DependencyInjection;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this WebApplicationBuilder builder)
    {

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });

        builder.AddSwaggerAuthorizationApiKeyDependencyInjection();
        builder.AddSwaggerAuthorizationJWTDependencyInjection();
        builder.AddAuthenticationJwtExtension();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddFluentValidationAutoValidation();
        //builder.Services.AddFluentValidationClientsideAdapters();
        //builder.Services.AddValidatorsFromAssembly(typeof(AccountRequestValidator).Assembly);


        //Services
        builder.Services.AddScoped<IValidator<AccountRequestDTO>, AccountRequestValidator>();
        builder.Services.AddScoped<ICorrelationId, CorrelationId>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<ITokenService, TokenService>();

        // Repositories & Data
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IMongoDbContext, MongoDbContext>();
    }
}
