using AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
namespace AuthService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Swagger-документ для контроллеров в Area("Web")
            //c.SwaggerDoc("web", new OpenApiInfo { Title = "Web API", Version = "v1" });

            // Swagger-документ для контроллеров вне area (например, SyncController и прочие)
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "General API", Version = "v1" });

            // Включать контроллеры в нужный документ на основе их area
            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                apiDesc.ActionDescriptor.RouteValues.TryGetValue("area", out var area);

                // doc "web" показывает только контроллеры из Area("Web")
                //if (docName.Equals("web", StringComparison.OrdinalIgnoreCase))
                //    return string.Equals(area, "Web", StringComparison.OrdinalIgnoreCase);

                // doc "v1" — всё без area
                if (docName.Equals("v1", StringComparison.OrdinalIgnoreCase))
                    return string.IsNullOrEmpty(area);

                return false;
            });


            // XML комментарии (опционально, если есть .xml-файл документации)
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            //c.SwaggerEndpoint("/swagger/web/swagger.json", "Web API v1");
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "General API v1");

            c.DocumentTitle = "Odyssey API Documentation";
            c.DocExpansion(DocExpansion.List);
            c.RoutePrefix = "swagger";
        });

        return app;
    }

    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AuthDbContext>>();

        try
        {
            var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

            var pending = await db.Database.GetPendingMigrationsAsync();
            if (pending.Any())
            {
                logger.LogInformation("Applying {Count} pending migrations...", pending.Count());
                await db.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("No pending migrations.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying migrations.");
            throw;
        }
    }
}
