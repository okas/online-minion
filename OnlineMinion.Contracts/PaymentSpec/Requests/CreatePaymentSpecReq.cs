using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public sealed class CreatePaymentSpecReq(string name, string currencyCode, string? tags)
    : BaseUpsertPaymentSpecReqData(name, currencyCode, tags), ICreateCommand
{
    public CreatePaymentSpecReq() : this(string.Empty, string.Empty, null) { }
}
