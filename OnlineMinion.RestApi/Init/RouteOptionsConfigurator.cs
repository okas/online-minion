using JetBrains.Annotations;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace OnlineMinion.RestApi.Init;

[UsedImplicitly]
public class RouteOptionsConfigurator : IConfigureOptions<RouteOptions>
{
    public void Configure(RouteOptions options)
    {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
    }
}
