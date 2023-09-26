using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using OnlineMinion.Contracts;

namespace OnlineMinion.RestApi.Configuration;

public class ApiCorsOptionsConfigurator : IConfigureOptions<CorsOptions>
{
    public const string ExposedHeadersPagingMetaInfoPolicy = nameof(ExposedHeadersPagingMetaInfoPolicy);

    public const string BasePolicy = nameof(BasePolicy);

    public void Configure(CorsOptions options)
    {
        var basePolicy = GetOrSetupDefaultPolicy(options);
        SetupPagingMetaInfoPolicy(options, basePolicy);
    }

    private static CorsPolicy GetOrSetupDefaultPolicy(CorsOptions options)
    {
        options.DefaultPolicyName = BasePolicy;

        if (options.GetPolicy(BasePolicy) is { } basePolicy)
        {
            return basePolicy;
        }

        basePolicy = new();

        options.AddDefaultPolicy(basePolicy);

        return basePolicy;
    }

    private static void SetupPagingMetaInfoPolicy(CorsOptions options, CorsPolicy basePolicy)
    {
        var policy = new CorsPolicyBuilder(basePolicy)
            .WithExposedHeaders(
                CustomHeaderNames.PagingRows,
                CustomHeaderNames.PagingSize,
                CustomHeaderNames.PagingPages
            )
            .Build();

        options.AddPolicy(ExposedHeadersPagingMetaInfoPolicy, policy);
    }
}
