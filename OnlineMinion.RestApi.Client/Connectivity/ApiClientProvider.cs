using Microsoft.Extensions.Options;
using OnlineMinion.RestApi.Client.Settings;

namespace OnlineMinion.RestApi.Client.Connectivity;

/// <summary>
///     Encapsulates configured <see cref="HttpClient" /> to communicate with backend API and resource <see cref="Uri" />
///     -s.
/// </summary>
public class ApiClientProvider
{
    public readonly Uri ApiV1AccountSpecsUri = new("api/v1/AccountSpecs", UriKind.Relative);

    public readonly Uri ApiV1CurrencyInfo = new("api/v1/CurrencyInfo", UriKind.Relative);

    public readonly Uri ApiV1PaymentSpecsUri = new("api/v1/PaymentSpecs", UriKind.Relative);

    public readonly Uri ApiV1TransactionsCreditUri = new("api/v1/Transactions/Credits", UriKind.Relative);

    public readonly Uri ApiV1TransactionsDebitUri = new("api/v1/Transactions/Debits", UriKind.Relative);

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
