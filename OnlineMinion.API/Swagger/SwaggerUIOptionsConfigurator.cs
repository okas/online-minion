using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace OnlineMinion.API.Swagger;

public class SwaggerUIOptionsConfigurator : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
    private readonly IWebHostEnvironment _webHostEnv;

    public SwaggerUIOptionsConfigurator(
        IWebHostEnvironment            webHostEnv,
        IApiVersionDescriptionProvider apiVersionDescriptionProvider
    )
    {
        _webHostEnv = webHostEnv;
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }

    public void Configure(SwaggerUIOptions options)
    {
        var applicationName = _webHostEnv.ApplicationName;

        options.DocumentTitle = $"{applicationName} | {options.DocumentTitle}";

        options.EnableFilter();
        options.ShowExtensions();
        options.DisplayRequestDuration();
        options.ShowCommonExtensions();

        // Allows multiple versions of our routes.
        // .Reverse(), first shown most recent version.
        foreach (var versionDescription in _apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint(
                $"/swagger/{versionDescription.GroupName}/swagger.json",
                $"{applicationName} - {versionDescription.GroupName.ToUpper()}"
            );
        }
    }
}
