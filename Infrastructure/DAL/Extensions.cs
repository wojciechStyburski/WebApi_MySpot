using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DAL.Decorators;
using MySpot.Infrastructure.DAL.Repositories;

namespace MySpot.Infrastructure.DAL;

internal static class Extensions
{
    private const string SectionName = "database";
    public static IServiceCollection AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(SectionName);
        services.Configure<DatabaseOptions>(section);
        var dbOptions = configuration.GetOptions<DatabaseOptions>(SectionName);

        services.AddDbContext<MySpotsDbContext>(options => options.UseSqlServer(dbOptions.ConnectionString));
        services.AddScoped<IWeeklyParkingSpotRepository, DatabaseWeeklyParkingSpotRepository>();
        services.AddScoped<IUserRepository, DatabaseUserRepository>();
        services.AddScoped<IUnitOfWork, DatabaseUnitOfWork>();
        services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
        services.AddHostedService<DatabaseInitializer>();

        return services;
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetSection(sectionName);
        section.Bind(options);

        return options;
    }
}