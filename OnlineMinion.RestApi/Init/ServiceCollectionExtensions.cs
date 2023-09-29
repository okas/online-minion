using CorsPolicySettings;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OnlineMinion.RestApi.Configuration;
using OnlineMinion.RestApi.ProblemHandling;
using OnlineMinion.RestApi.Services.LinkGeneration;
using OnlineMinion.RestApi.Swagger;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureRestApi(
        this IServiceCollection services,
        ConfigurationManager    confManager,
        IWebHostEnvironment     environment
    )
    {
        #region CORS

        // Order is important (CORS): first read base policies, then produce CORS configuration.
        services.AddCorsPolicies(confManager);

        services.ConfigureOptions<ApiCorsOptionsConfigurator>().AddCors();

        #endregion

        #region API general setup

        services.ConfigureOptions<RouteOptionsConfigurator>();

        services.AddHttpContextAccessor().AddScoped<ResourceLinkGenerator>();

        #endregion

        #region API Problem handling setup

        services.AddSingleton<IExceptionProblemDetailsMapper, ApiExceptionProblemDetailsMapper>()
            .ConfigureOptions<ProblemDetailsOptionsConfigurator>()
            .AddProblemDetails();

        #endregion

        # region API versioning setup

        services.ConfigureOptions<ApiVersioningOptionsConfigurator>()
            .ConfigureOptions<ApiExplorerOptionsConfigurator>()
            .AddEndpointsApiExplorer()
            .AddApiVersioning()
            .AddApiExplorer();

        #endregion

        #region Application services

        services.AddApplication();

        #endregion

        #region Swagger setup

        services.AddSwaggerGen(confManager, environment);

        #endregion

        return services;
    }

    private static IServiceCollection AddSwaggerGen(
        this IServiceCollection services,
        ConfigurationManager    confManager,
        IWebHostEnvironment     environment
    )
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.Configure<SwaggerOptions>(confManager.GetSection(nameof(SwaggerOptions)))
            .Configure<SwaggerGeneratorOptions>(confManager.GetSection(nameof(SwaggerGeneratorOptions)))
            .Configure<SchemaGeneratorOptions>(confManager.GetSection(nameof(SchemaGeneratorOptions)))
            .Configure<SwaggerGenOptions>(confManager.GetSection(nameof(SwaggerGenOptions)));

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

        if (environment.IsDevelopment())
        {
            services.Configure<SwaggerUIOptions>(confManager.GetSection(nameof(SwaggerUIOptions)))
                .ConfigureOptions<SwaggerUIOptionsConfigurator>();
        }

        return services;
    }
}
