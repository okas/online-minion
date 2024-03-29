using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OnlineMinion.Application.Contracts;

namespace OnlineMinion.RestApi.Configuration;

public class ApiVersioningOptionsConfigurator : IConfigureOptions<ApiVersioningOptions>
{
    public void Configure(ApiVersioningOptions options)
    {
        options.AssumeDefaultVersionWhenUnspecified = false;
        options.UnsupportedApiVersionStatusCode = StatusCodes.Status400BadRequest;
        options.ReportApiVersions = true;

        options.ApiVersionReader = ApiVersionReader.Combine(
            new HeaderApiVersionReader(CustomHeaderNames.ApiVersion)
        );
    }
}
