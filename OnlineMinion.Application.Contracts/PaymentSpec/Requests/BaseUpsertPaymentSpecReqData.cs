namespace OnlineMinion.Application.Contracts.PaymentSpec.Requests;

public abstract class BaseUpsertPaymentSpecReqData(string name, string currencyCode, string? tags)
{
    public string Name { get; set; } = name;

    public string CurrencyCode { get; set; } = currencyCode;

    public string? Tags { get; set; } = tags;
}
