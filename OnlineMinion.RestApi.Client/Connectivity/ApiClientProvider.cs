using Microsoft.Extensions.Options;
using OnlineMinion.RestApi.Client.Settings;

namespace OnlineMinion.RestApi.Client.Connectivity;

/// <summary>
///     Encapsulates configured <see cref="HttpClient" /> to communicate with backend API and resource
///     <see cref="Uri" />     -s.
/// </summary>
public class ApiClientProvider
{
    public readonly Uri ApiV1AccountSpecsUri = new("api/account-specs", UriKind.Relative);

    public readonly Uri ApiV1CurrencyInfo = new("api/currency-info", UriKind.Relative);

    public readonly Uri ApiV1PaymentSpecsUri = new("api/PaymentSpecs", UriKind.Relative);

    public readonly Uri ApiV1TransactionsCreditUri = new("api/Transactions/Credits", UriKind.Relative);

    public readonly Uri ApiV1TransactionsDebitUri = new("api/Transactions/Debits", UriKind.Relative);

    public ApiClientProvider(HttpClient httpClient, IOptions<ApiClientProviderSettings> options)
    {
        httpClient.BaseAddress = new(
            options.Value.Url
            ?? throw new InvalidOperationException(GetMsgMissingConfig())
        );

        Client = httpClient;
    }

    public HttpClient Client { get; }

    private static string GetMsgMissingConfig() => $"Missing {nameof(ApiClientProviderSettings)} from configuration.";
}
