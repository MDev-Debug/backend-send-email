using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SendEmail.Domain.Constantes.Auth;
using SendEmail.Domain.DTOs;
using SendEmail.Domain.Enum;
using SendEmail.Domain.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace SendEmail.CrossCutting.MiddleWare;

public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _scretApiKey = "";
    private readonly bool _hmacEnable = false;
    private readonly ILogger<ApiKeyAuthMiddleware> _logger;

    public ApiKeyAuthMiddleware(RequestDelegate next, ILogger<ApiKeyAuthMiddleware> logger)
    {
        _next = next;
        var scretApiKey = Environment.GetEnvironmentVariable("SECRET_API_KEY");
        var hmacEnable = Environment.GetEnvironmentVariable("HMAC_ENABLED");

        if (!string.IsNullOrWhiteSpace(scretApiKey))
            _scretApiKey = scretApiKey;

        if (!string.IsNullOrWhiteSpace(hmacEnable) && hmacEnable == "1")
            _hmacEnable = true;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ICorrelationId correlationId)
    {
        if (_hmacEnable)
        {
            var publicApiKey = context.Request.Headers[ApiKeyAuth.HeaderPublicApiKey];
            var hashHmacClient = context.Request.Headers[ApiKeyAuth.HeaderSecretKeyHmac];

            if (string.IsNullOrWhiteSpace(publicApiKey) || string.IsNullOrWhiteSpace(hashHmacClient))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync($"{new Response(correlationId.ObterCorrelationId(), "API Key missig", EResponse.UNAUTHORIZED)}");
                return;
            }

            if (!IsHashValid(publicApiKey!, hashHmacClient!))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync($"{new Response(correlationId.ObterCorrelationId(), "API Key missig", EResponse.UNAUTHORIZED)}");
                return;
            }
        }

        await _next(context);
    }

    private bool IsHashValid(string publicApiKeyClient, string hashHmacClient)
    {
        string secretKey = Environment.GetEnvironmentVariable("SECRET_API_KEY");
        string message = $"{publicApiKeyClient}_{secretKey}";
        var hashBytes = Encoding.UTF8.GetBytes(message);
        var base64 = Convert.ToBase64String(hashBytes);
        string hash = GenerateHMACSHA256(secretKey, base64);

        var isValid = hash.Equals(hashHmacClient);

        _logger.LogInformation("Is valid {0}", isValid);
        return isValid;
    }

    private static string GenerateHMACSHA256(string key, string message)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        using (var hmacsha256 = new HMACSHA256(keyBytes))
        {
            byte[] hashBytes = hmacsha256.ComputeHash(messageBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
