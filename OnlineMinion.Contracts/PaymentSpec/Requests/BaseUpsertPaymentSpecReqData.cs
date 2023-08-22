namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public abstract class BaseUpsertPaymentSpecReqData
{
    protected BaseUpsertPaymentSpecReqData(string name, string currencyCode, string? tags)
    {
        Name = name;
        CurrencyCode = currencyCode;
        Tags = tags;
    }

    public string Name { get; set; }

    public string CurrencyCode { get; set; }

    public string? Tags { get; set; }
}
