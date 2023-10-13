using Microsoft.EntityFrameworkCore;
using OnlineMinion.Application;
using OnlineMinion.DataStore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private static readonly string MigrationsAssemblyName = typeof(OnlineMinionDbContext).Assembly.FullName!;

    public static IServiceCollection AddDataStore(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<IOnlineMinionDbContext, OnlineMinionDbContext>(
            dbContextOptionsBuilder =>
                dbContextOptionsBuilder.UseSqlServer(
                    connectionString,
                    serverDbContextOptionsBuilder => serverDbContextOptionsBuilder
                        .MigrationsAssembly(MigrationsAssemblyName)
                )
        );

        return services;
    }
}
