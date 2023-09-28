using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using OnlineMinion.AppService.Swagger;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerGen(
        this IServiceCollection services,
        IConfigurationRoot      configurationRoot
    )
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.Configure<SwaggerOptions>(configurationRoot.GetSection(nameof(SwaggerOptions)))
            .Configure<SwaggerGeneratorOptions>(configurationRoot.GetSection(nameof(SwaggerGeneratorOptions)))
            .Configure<SchemaGeneratorOptions>(configurationRoot.GetSection(nameof(SchemaGeneratorOptions)))
            .Configure<SwaggerGenOptions>(configurationRoot.GetSection(nameof(SwaggerGenOptions)));

        services.ConfigureOptions<SwaggerGenOptionsConfigurator>();

        services.AddSwaggerGen()
            .AddFluentValidationRulesToSwagger(
                options => options.ValidatorSearch = new()
                {
                    IsOneValidatorForType = false,
                    SearchBaseTypeValidators =
                        false, // This aligns with the behavior of default MS DI  Service resolution works!
                    // Enforces to use only one validator per type and combine them using 'Include' rules.
                }
            );

        return services;
    }

    public static IServiceCollection AddSwaggerUI(
        this IServiceCollection services,
        IConfigurationRoot      configurationRoot
    )
    {
        services.Configure<SwaggerUIOptions>(configurationRoot.GetSection(nameof(SwaggerUIOptions)))
            .ConfigureOptions<SwaggerUIOptionsConfigurator>();

        return services;
    }
}
