using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpec.Requests;

public sealed class CreatePaymentSpecReq(string name, string currencyCode, string? tags)
    : BaseUpsertPaymentSpecReqData(name, currencyCode, tags), ICreateCommand
{
    public CreatePaymentSpecReq() : this(string.Empty, string.Empty, null) { }
}
