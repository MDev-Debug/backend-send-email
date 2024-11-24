using Microsoft.AspNetCore.Http;
using SendEmail.Domain.Interfaces.Services;

namespace SendEmail.CrossCutting.MiddleWare;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICorrelationId correlationId)
    {
        correlationId.GerarCorrelationId();
        await _next(context);
    }
}
