using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OnlineMinion.RestApi.Configuration;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OnlineMinion.Application.Swagger;

public class SwaggerGenOptionsConfigurator(IApiVersionDescriptionProvider apiVersionDescriptionProvider, IServer server)
    : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IServerAddressesFeature _addressFeature =
        server.Features.Get<IServerAddressesFeature>()
        ?? throw new InvalidOperationException(
            "Cannot obtain assembly name for SwaggerGenOptions configuration."
        );

    private readonly string _apiAssemblyName =
        typeof(ServicesSetup).Assembly.GetName().Name
        ?? throw new InvalidOperationException(
            "Cannot obtain server address feature for SwaggerGenOptions configuration."
        );

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

        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
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
        CheckServers(options);
    }

    private void CheckServers(SwaggerGenOptions options)
    {
        var server = options.SwaggerGeneratorOptions.Servers;

        foreach (var x in _addressFeature.Addresses)
        {
            if (!server.Exists(apiSrv => string.Equals(apiSrv.Url, x, StringComparison.OrdinalIgnoreCase)))
            {
                server.Add(new() { Url = x, });
            }
        }
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
