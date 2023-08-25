using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using OnlineMinion.Contracts;

namespace OnlineMinion.RestApi.Configuration;

public class ApiCorsOptionsConfigurator : IConfigureOptions<CorsOptions>
{
    public const string ExposedHeadersPagingMetaInfo = "ExposedHeadersPagingMetaInfo";

    public const string DefaultPolicyName = "BasePolicy";

    public void Configure(CorsOptions options)
    {
        var basePolicy = GetOrCreateDefaultPolicy(options);

        var specificPolicyBuilder = new CorsPolicyBuilder(basePolicy)
            .WithExposedHeaders(
                CustomHeaderNames.PagingRows,
                CustomHeaderNames.PagingSize,
                CustomHeaderNames.PagingPages
            );

        options.DefaultPolicyName = DefaultPolicyName;

        options.AddPolicy(ExposedHeadersPagingMetaInfo, specificPolicyBuilder.Build());
    }

    private static CorsPolicy GetOrCreateDefaultPolicy(CorsOptions options)
    {
        if (options.GetPolicy(DefaultPolicyName) is { } basePolicy)
        {
            return basePolicy;
        }

        basePolicy = new();
        options.AddDefaultPolicy(basePolicy);

        return basePolicy;
    }
}
