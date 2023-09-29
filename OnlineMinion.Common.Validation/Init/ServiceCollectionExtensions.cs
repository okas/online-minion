using FluentValidation;
using OnlineMinion.Common.Validation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IAssemblyMarkerCommonValidation>();

        return services;
    }
}
