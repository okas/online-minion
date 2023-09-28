using Microsoft.Extensions.DependencyInjection;
using OnlineMinion.Common;

namespace OnlineMinion.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IAsyncValidatorSender, MediatorWrapper>();

        return services;
    }
}
