using Microsoft.Extensions.Options;
using OnlineMinion.RestApi.Client.Settings;

namespace OnlineMinion.RestApi.Client.Infrastructure;

/// <summary>
///     Encapsulates configured <see cref="HttpClient" /> to communicate with backend API and resource <see cref="Uri" />
///     -s.
/// </summary>
public class ApiClientProvider
{
    public readonly Uri ApiV1AccountSpecsUri = new("api/v1/AccountSpecs", UriKind.Relative);

    public ApiClientProvider(HttpClient httpClient, IOptions<ApiClientProviderSettings> options)
    {
        httpClient.BaseAddress = new(
            options.Value.Url ??
            throw new InvalidOperationException($"Missing {nameof(ApiClientProviderSettings)} from configuration.")
        );

        Client = httpClient;
    }

    public HttpClient Client { get; }
}
