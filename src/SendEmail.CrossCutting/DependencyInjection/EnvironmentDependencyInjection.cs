using Microsoft.AspNetCore.Builder;
using SendEmail.CrossCutting.MiddleWare;

namespace SendEmail.CrossCutting.DependencyInjection;

public static class EnvironmentDependencyInjection
{
    public static void AddEnvironmentDependencyInjection(this WebApplication app)
    {
        var env = app.Environment.EnvironmentName;

        if (env == "DEV" || env == "HML")
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ApiKeyAuthMiddleware>();
    }
}
