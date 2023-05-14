using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using OnlineMinion.RestApi;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace OnlineMinion.Application.Swagger;

public class SwaggerUIOptionsConfigurator : IConfigureOptions<SwaggerUIOptions>
{
    private readonly string _apiAssemblyName;
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

    public SwaggerUIOptionsConfigurator(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        _apiAssemblyName = typeof(AccountSpecsController).Assembly.GetName().Name ??
                           throw new InvalidOperationException();

        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }

    public void Configure(SwaggerUIOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.DocumentTitle))
        {
            options.DocumentTitle = _apiAssemblyName;
        }

        // Allows multiple versions of our routes.
        // .Reverse(), first shown most recent version.
        foreach (var versionDescription in _apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint(
                $"/swagger/{versionDescription.GroupName}/swagger.json",
                $"{_apiAssemblyName} - {versionDescription.GroupName.ToUpper()}"
            );
        }
    }
}
