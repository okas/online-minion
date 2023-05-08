using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using OnlineMinion.Contracts.HttpHeaders;

namespace OnlineMinion.API.Configurators;

public class ApiCorsOptionsConfigurator : IConfigureOptions<CorsOptions>
{
    public const string ExposedHeadersPagingMetaInfo = "ExposedHeadersPagingMetaInfo";
    public const string DefaultPolicyName = "BasePolicy";

    public void Configure(CorsOptions options)
    {
        var basePolicy = GetOrCreateDefaultPolicy(options);

        var specificPolicyBuilder = new CorsPolicyBuilder(basePolicy)
            .WithExposedHeaders(
                CustomHeaderNames.PagingTotalItems,
                CustomHeaderNames.PagingSize,
                CustomHeaderNames.PagingPages
            );

        options.DefaultPolicyName = DefaultPolicyName;

        options.AddPolicy(ExposedHeadersPagingMetaInfo, specificPolicyBuilder.Build());
    }

    private static CorsPolicy GetOrCreateDefaultPolicy(CorsOptions options)
    {
        var basePolicy = options.GetPolicy(DefaultPolicyName);
        if (basePolicy is not null)
        {
            return basePolicy;
        }

        basePolicy = new();
        options.AddDefaultPolicy(basePolicy);

        return basePolicy;
    }
}
