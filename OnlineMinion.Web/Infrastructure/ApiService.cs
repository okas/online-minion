using Microsoft.Extensions.Options;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.Infrastructure;

/// <summary>
///     Encapsulates configured <see cref="HttpClient"/> to communicate with backend API and resource <see cref="Uri"/>-s.
/// </summary>
public class ApiService
{
    public readonly Uri ApiV1AccountSpecsUri = new("api/v1/AccountSpecs", UriKind.Relative);

    public ApiService(HttpClient httpClient, IOptions<ApiServiceSettings> options)
    {
        httpClient.BaseAddress = new(
            options.Value.Url ??
            throw new InvalidOperationException($"Missing {nameof(ApiServiceSettings)} from configuration.")
        );

        Client = httpClient;
    }

    public HttpClient Client { get; }
}
