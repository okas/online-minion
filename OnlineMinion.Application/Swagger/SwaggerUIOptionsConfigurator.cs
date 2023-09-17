using System.Globalization;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using OnlineMinion.RestApi;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace OnlineMinion.Application.Swagger;

public class SwaggerUIOptionsConfigurator(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    : IConfigureOptions<SwaggerUIOptions>
{
    private readonly string _apiAssemblyName =
        typeof(AccountSpecsController).Assembly.GetName().Name
        ?? throw new InvalidOperationException();

    public void Configure(SwaggerUIOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.DocumentTitle))
        {
            options.DocumentTitle = _apiAssemblyName;
        }

        // Allows multiple versions of our routes.
        // .Reverse(), first shown most recent version.
        foreach (var versionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint(
                $"/swagger/{versionDescription.GroupName}/swagger.json",
                $"{_apiAssemblyName} - {versionDescription.GroupName.ToUpper(CultureInfo.InvariantCulture)}"
            );
        }

        SetCustomCss(options);
    }

    private static void SetCustomCss(SwaggerUIOptions options)
    {
        const string key = "CustomCssPath";
        if (!options.ConfigObject.AdditionalItems.TryGetValue(key, out var val) || val is not string cssPath)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(cssPath))
        {
            options.InjectStylesheet(cssPath);
        }
    }
}
