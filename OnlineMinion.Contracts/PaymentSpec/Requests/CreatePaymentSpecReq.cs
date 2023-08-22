using OnlineMinion.Contracts.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public sealed class CreatePaymentSpecReq : BaseUpsertPaymentSpecReqData, ICreateCommand
{
    public CreatePaymentSpecReq() : base(string.Empty, string.Empty, null) { }

    public CreatePaymentSpecReq(string name, string currencyCode, string? tags) : base(name, currencyCode, tags) { }
}
