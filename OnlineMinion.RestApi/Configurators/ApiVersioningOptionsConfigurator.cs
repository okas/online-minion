using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;

namespace OnlineMinion.RestApi.Configurators;

public class ApiVersioningOptionsConfigurator : IConfigureOptions<ApiVersioningOptions>
{
    public void Configure(ApiVersioningOptions options)
    {
        options.AssumeDefaultVersionWhenUnspecified = true;

        options.ReportApiVersions = true;

        options.ApiVersionReader = ApiVersionReader.Combine(
            new QueryStringApiVersionReader("api-ver"),
            new HeaderApiVersionReader("X-Version"),
            new MediaTypeApiVersionReader("ver"),
            new UrlSegmentApiVersionReader()
        );
    }
}
