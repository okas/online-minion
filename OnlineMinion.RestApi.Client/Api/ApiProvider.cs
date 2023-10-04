namespace OnlineMinion.RestApi.Client.Api;

/// <summary>
///     Encapsulates configured <see cref="HttpClient" /> to communicate with backend API and resource
///     <see cref="Uri" />-s.
/// </summary>
public record ApiProvider(HttpClient Client)
{
    public static readonly Uri ApiAccountSpecsUri = new("api/account-specs", UriKind.Relative);

    public static readonly Uri ApiCurrencyInfoUri = new("api/currency-info", UriKind.Relative);

    public static readonly Uri ApiPaymentSpecsUri = new("api/payment-specs/cash", UriKind.Relative);

    public static readonly Uri ApiTransactionsCreditUri = new("api/transactions/credits", UriKind.Relative);

    public static readonly Uri ApiTransactionsDebitUri = new("api/transactions/debits", UriKind.Relative);
}
