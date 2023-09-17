using Asp.Versioning;
using Microsoft.Extensions.Options;

namespace OnlineMinion.RestApi.Configuration;

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
