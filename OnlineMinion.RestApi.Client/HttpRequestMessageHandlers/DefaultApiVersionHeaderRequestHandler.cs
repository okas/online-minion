using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using OnlineMinion.Contracts;
using OnlineMinion.RestApi.Client.Settings;

namespace OnlineMinion.RestApi.Client.HttpRequestMessageHandlers;

/// <summary>
///     If the request does not contain the <see cref="CustomHeaderNames.ApiVersion">CustomHeaderNames.ApiVersion</see>
///     header, then this handler will add it with the value from Configuration,
///     <see cref="ApiProviderSettings.DefaultApiVersion">ApiProviderSettings.DefaultApiVersion</see>.
/// </summary>
/// <param name="options">DI injected options to get default API Version.</param>
[UsedImplicitly]
public class DefaultApiVersionHeaderRequestHandler(IOptions<ApiProviderSettings> options) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        if (!request.Headers.Contains(CustomHeaderNames.ApiVersion))
        {
            request.Headers.Add(CustomHeaderNames.ApiVersion, options.Value.DefaultApiVersion);
        }

        return await base.SendAsync(request, ct);
    }
}
