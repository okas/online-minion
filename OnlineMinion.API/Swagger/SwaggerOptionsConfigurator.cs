using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OnlineMinion.API.Swagger;

public class SwaggerGenOptionsConfiguration : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
    private readonly IWebHostEnvironment _webHostEnv;

    public SwaggerGenOptionsConfiguration(
        IWebHostEnvironment            webHostEnv,
        IApiVersionDescriptionProvider apiVersionDescriptionProvider
    )
    {
        _webHostEnv = webHostEnv;
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        options.IncludeXmlComments(
            Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"),
            true
        );

        options.EnableAnnotations();
        options.SupportNonNullableReferenceTypes();
        options.OperationFilter<SwaggerDefaultValuesFilter>();
        options.OperationFilter<AddResponseHeadersFilter>();
        options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

        // TODO: Add authentication stuffs
        //options.AddSecurityDefinition(...);
        //options.AddSecurityRequirement(...);

        foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
            options.OrderActionsBy(apiDesc => $"{apiDesc.RelativePath}");
            options.DescribeAllParametersInCamelCase();
            options.CustomSchemaIds(DefaultSchemaIdSelector);
        }
    }

    private OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description) => new()
    {
        Title = _webHostEnv.ApplicationName,
        Version = description.ApiVersion.ToString(),
        Description = $"Nummi API :-){(description.IsDeprecated ? " (deprecated)" : string.Empty)}",
        Contact = new()
        {
            Name = "Lauri Saar",
            Email = "lauri.saar@outlook.com"
        }
    };

    private static string DefaultSchemaIdSelector(Type modelType)
    {
        if (!modelType.IsConstructedGenericType)
        {
            return modelType.Name;
        }

        var prefix = modelType.GetGenericArguments()
            .Select(DefaultSchemaIdSelector)
            .Aggregate((previous, current) => previous + current);

        return modelType.Name.Split('`').First() + "Of" + prefix;
    }
}
