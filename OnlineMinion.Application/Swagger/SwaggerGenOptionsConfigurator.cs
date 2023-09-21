using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OnlineMinion.RestApi.Init;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OnlineMinion.Application.Swagger;

/// <inheritdoc />
public class SwaggerGenOptionsConfigurator(IApiVersionDescriptionProvider apiVersionDescriptionProvider, IServer server)
    : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IServerAddressesFeature _addressFeature = GetAddressFeature(server);
    private readonly string _apiAssemblyName = GetApiAssemblyName();

    public void Configure(SwaggerGenOptions options)
    {
        options.IncludeXmlComments(
            Path.Combine(AppContext.BaseDirectory, $"{_apiAssemblyName}.xml"),
            true
        );

        options.OperationFilter<SwaggerDefaultValuesOperationFilter>();
        options.OperationFilter<AddResponseHeadersFilter>();
        options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

        // TODO: Add authentication stuffs
        //options.AddSecurityDefinition(...);
        //options.AddSecurityRequirement(...);

        CreateOrUpdateOpenApiDocs(apiVersionDescriptionProvider, options);

        options.OrderActionsBy(apiDesc => $"{apiDesc.RelativePath}");
        options.DescribeAllParametersInCamelCase();
        options.CustomSchemaIds(DefaultSchemaIdSelector);

        CheckContacts(options);
        CheckLicense(options);
        CheckServers(options);
    }

    private void CreateOrUpdateOpenApiDocs(IApiVersionDescriptionProvider provider, SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            if (options.SwaggerGeneratorOptions.SwaggerDocs.TryGetValue(description.GroupName, out var doc))
            {
                SetGeneralApiInfoToExistingDoc(doc, description);
            }
            else
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
            }
        }
    }

    private void SetGeneralApiInfoToExistingDoc(OpenApiInfo doc, ApiVersionDescription versionDescription)
    {
        if (string.IsNullOrWhiteSpace(doc.Title))
        {
            doc.Title = _apiAssemblyName;
        }

        if (string.IsNullOrWhiteSpace(doc.Version))
        {
            doc.Version = versionDescription.ApiVersion.ToString();
        }

        if (versionDescription.IsDeprecated)
        {
            doc.Description = GenerateDescriptionUsingSunSetPolicyInfo(versionDescription, doc.Description);
        }
    }

    private OpenApiInfo CreateOpenApiInfo(ApiVersionDescription versionDescription) => new()
    {
        Title = _apiAssemblyName,
        Version = versionDescription.ApiVersion.ToString(),
        Description = GenerateDescriptionUsingSunSetPolicyInfo(versionDescription),
    };

    private static string GenerateDescriptionUsingSunSetPolicyInfo(
        ApiVersionDescription versionDescription,
        string                existingDescription = ""
    )
    {
        var sb = new StringBuilder(existingDescription);

        if (versionDescription.IsDeprecated)
        {
            sb.Append(" This API version has been deprecated.");
        }

        if (versionDescription.SunsetPolicy is not { } policy)
        {
            return sb.ToString();
        }

        if (policy.Date is { } when)
        {
            sb.Append(" The API will be sunset on ")
                .Append(when.Date.ToShortDateString())
                .Append('.');
        }

        if (!policy.HasLinks)
        {
            return sb.ToString();
        }

        sb.AppendLine();

        AppendLinksInfo(sb, policy);

        return sb.ToString();
    }

    private static void AppendLinksInfo(StringBuilder sb, SunsetPolicy policy)
    {
        foreach (var link in policy.Links.Where(l => l.Type == "text/html"))
        {
            sb.AppendLine();

            if (link.Title.HasValue)
            {
                sb.Append(link.Title.Value).Append(": ");
            }

            sb.Append(link.LinkTarget.OriginalString);
        }
    }

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

    private static void CheckLicense(SwaggerGenOptions options)
    {
        var apiInfos = options.SwaggerGeneratorOptions.SwaggerDocs.Values.ToList();

        var docsWithoutLicense = apiInfos
            .Where(i => i.License is null)
            .ToList();

        if (docsWithoutLicense.Count == apiInfos.Count || docsWithoutLicense.Count <= 0)
        {
            return;
        }

        var license = apiInfos.Last(i => i.License is not null).License;
        apiInfos.ForEach(i => i.License = license);
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

    private static IServerAddressesFeature GetAddressFeature(IServer server) =>
        server.Features.Get<IServerAddressesFeature>()
        ?? throw new InvalidOperationException(
            "Cannot obtain assembly name for SwaggerGenOptions configuration."
        );

    private static string GetApiAssemblyName() =>
        typeof(ServicesSetup).Assembly.GetName().Name
        ?? throw new InvalidOperationException(
            "Cannot obtain server address feature for SwaggerGenOptions configuration."
        );
}
