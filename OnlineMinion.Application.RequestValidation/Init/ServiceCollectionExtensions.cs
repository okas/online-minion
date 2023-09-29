using FluentValidation;
using OnlineMinion.Application.RequestValidation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationRequestValidation(this IServiceCollection services) =>
        services.AddValidatorsFromAssemblyContaining<IAssemblyMarkerCommonValidation>();
}
