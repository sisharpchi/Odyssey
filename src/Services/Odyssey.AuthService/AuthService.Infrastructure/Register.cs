using AuthService.Core.Entities;
using AuthService.Core.Entities.OpenId;
using AuthService.Core.Persistence;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Persistences;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenIddict.Abstractions;

namespace AuthService.Infrastructure;

public static class Register
{
    private const string DefaultConnectionSection = "auth-db";

    private static void AddDatabase(
        IServiceCollection services,
        IConfiguration configuration,
        string connectionSection,
        Action<DbContextOptionsBuilder>? optionsAction = null)
    {
        var connectionString = configuration.GetConnectionString(connectionSection);

        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        if (builder.Timeout <= 0)
            builder.Timeout = 60;
        connectionString = builder.ToString();

        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseOpenIddict<OpenIdApplication, OpenIdAuthorization, OpenIdScope, OpenIdToken, long>();
            optionsAction?.Invoke(options);
        });
    }

    private static void AddIdentity(IServiceCollection services, Action<IdentityBuilder>? configureIdentity = null)
    {
        var identityBuilder = services
            .AddIdentity<User, UserRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        configureIdentity?.Invoke(identityBuilder);
    }

    private static void AddOpenIddict(IServiceCollection services)
    {
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<AuthDbContext>()
                       .ReplaceDefaultEntities<OpenIdApplication, OpenIdAuthorization, OpenIdScope, OpenIdToken, long>();
            })
            .AddServer(options =>
            {
                options.RegisterScopes(
                    "read",
                    "write",
                    OpenIddictConstants.Scopes.OfflineAccess);

                options.SetTokenEndpointUris("security/oauth/token")
                       .SetRevocationEndpointUris("security/oauth/revoke");

                options.AllowClientCredentialsFlow()
                       .AllowPasswordFlow()
                       .AllowRefreshTokenFlow();

                options.SetAccessTokenLifetime(TimeSpan.FromHours(24));
                options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                       .DisableTransportSecurityRequirement()
                       .EnableTokenEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });
    }

    private static void AddPersistence(IServiceCollection services)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork<AuthDbContext>>();
        services.AddScoped<IRepository>(sp => sp.GetRequiredService<IAuthRepository>());
    }

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IdentityBuilder>? configureIdentity = null,
        Action<DbContextOptionsBuilder>? optionsAction = null,
        string connectionSection = DefaultConnectionSection)
    {
        AddDatabase(services, configuration, connectionSection, optionsAction);
        AddIdentity(services, configureIdentity);
        AddOpenIddict(services);
        AddPersistence(services);

        return services;
    }
}