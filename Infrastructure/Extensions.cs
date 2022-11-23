using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Abstractions;
using MySpot.Core.Abstractions;
using MySpot.Infrastructure.Auth;
using MySpot.Infrastructure.DAL;
using MySpot.Infrastructure.Logging;
using MySpot.Infrastructure.Middlewares;
using MySpot.Infrastructure.Security;
using MySpot.Infrastructure.Time;

//[assembly: InternalsVisibleTo("MySpot.Tests.Unit")]
namespace MySpot.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<ExceptionMiddleware>()
            .AddSecurity()
            .AddDatabaseConnection(configuration)
            .AddSingleton<IClock, Clock>()
            .AddCustomLogging()
            .AddAuth(configuration)
            .AddHttpContextAccessor();

        var infrastructureAssembly = typeof(DatabaseOptions).Assembly;

        services
            .Scan(s => s.FromAssemblies(infrastructureAssembly)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app
            .UseMiddleware<ExceptionMiddleware>()
            .UseAuthentication()
            .UseAuthorization();

        return app;
    }
}