using ErrorOr;
using MediatR;
using OnlineMinion.Contracts.AppMessaging;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public sealed class CreatePaymentSpecReq : BaseUpsertPaymentSpecReqData, IRequest<ErrorOr<ModelIdResp>>
{
    public CreatePaymentSpecReq() : base(string.Empty, string.Empty, null) { }

    public CreatePaymentSpecReq(string name, string currencyCode, string? tags) : base(name, currencyCode, tags) { }
}
