using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace OnlineMinion.RestApi.Init;

public class ApiVersioningOptionsConfigurator : IConfigureOptions<ApiVersioningOptions>
{
    public void Configure(ApiVersioningOptions options)
    {
        options.AssumeDefaultVersionWhenUnspecified = false;
        options.UnsupportedApiVersionStatusCode = StatusCodes.Status400BadRequest;
        options.ReportApiVersions = true;

        options.ApiVersionReader = ApiVersionReader.Combine(
            new QueryStringApiVersionReader("api-ver"),
            new HeaderApiVersionReader("X-Version"),
            new MediaTypeApiVersionReader("ver"),
            new UrlSegmentApiVersionReader()
        );
    }
}
