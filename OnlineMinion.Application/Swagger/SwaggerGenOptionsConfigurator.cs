using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OnlineMinion.RestApi;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OnlineMinion.Application.Swagger;

public class SwaggerGenOptionsConfigurator : IConfigureOptions<SwaggerGenOptions>
{
    private readonly string _apiAssemblyName;
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

    public SwaggerGenOptionsConfigurator(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        _apiAssemblyName = typeof(AccountSpecsController).Assembly.GetName().Name ??
                           throw new InvalidOperationException();

        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        options.IncludeXmlComments(
            Path.Combine(AppContext.BaseDirectory, $"{_apiAssemblyName}.xml"),
            true
        );

        options.OperationFilter<SwaggerDefaultValuesFilter>();
        options.OperationFilter<AddResponseHeadersFilter>();
        options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

        // TODO: Add authentication stuffs
        //options.AddSecurityDefinition(...);
        //options.AddSecurityRequirement(...);

        foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            if (options.SwaggerGeneratorOptions.SwaggerDocs.TryGetValue(description.GroupName, out var doc))
            {
                CheckApiInfo(doc, description);
            }
            else
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
            }

            options.OrderActionsBy(apiDesc => $"{apiDesc.RelativePath}");
            options.DescribeAllParametersInCamelCase();
            options.CustomSchemaIds(DefaultSchemaIdSelector);
        }

        CheckContacts(options);
    }

    private void CheckApiInfo(OpenApiInfo doc, ApiVersionDescription description)
    {
        if (string.IsNullOrWhiteSpace(doc.Title))
        {
            doc.Title = _apiAssemblyName;
        }

        if (string.IsNullOrWhiteSpace(doc.Version))
        {
            doc.Version = description.ApiVersion.ToString();
        }

        if (description.IsDeprecated)
        {
            doc.Description = $"{doc.Description} (deprecated)".Trim();
        }
    }

    private OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description) => new()
    {
        Title = _apiAssemblyName,
        Version = description.ApiVersion.ToString(),
        Description = $"{(description.IsDeprecated ? "(deprecated)" : string.Empty)}",
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

    private static void CheckContacts(SwaggerGenOptions options)
    {
        var apiInfos = options.SwaggerGeneratorOptions.SwaggerDocs.Values.ToList();

        var docsWithoutContact = apiInfos
            .Where(i => i.Contact is null)
            .ToList();

        if (docsWithoutContact.Count == apiInfos.Count || docsWithoutContact.Count <= 0)
        {
            return;
        }

        var contact = apiInfos.Last(i => i.Contact is not null).Contact;
        apiInfos.ForEach(i => i.Contact = contact);
    }
}
