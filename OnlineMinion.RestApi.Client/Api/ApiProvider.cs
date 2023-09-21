namespace OnlineMinion.RestApi.Client.Api;

/// <summary>
///     Encapsulates configured <see cref="HttpClient" /> to communicate with backend API and resource
///     <see cref="Uri" />-s.
/// </summary>
public class ApiProvider(HttpClient httpClient)
{
    public readonly Uri ApiAccountSpecsUri = new("api/account-specs", UriKind.Relative);

    public readonly Uri ApiCurrencyInfoUri = new("api/currency-info", UriKind.Relative);

    public readonly Uri ApiPaymentSpecsUri = new("api/PaymentSpecs", UriKind.Relative);

    public readonly Uri ApiTransactionsCreditUri = new("api/Transactions/Credits", UriKind.Relative);

    public readonly Uri ApiTransactionsDebitUri = new("api/Transactions/Debits", UriKind.Relative);

    public HttpClient Client { get; } = httpClient;
}
