namespace OnlineMinion.Application.Contracts.PaymentSpec.Requests;

public abstract class BaseUpsertPaymentSpecReqData(string name, string? tags)
{
    public string Name { get; set; } = name;

    public string? Tags { get; set; } = tags;
}
