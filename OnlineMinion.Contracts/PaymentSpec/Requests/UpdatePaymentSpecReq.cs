using ErrorOr;
using MediatR;
using OnlineMinion.Contracts.AppMessaging;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public sealed class UpdatePaymentSpecReq : BaseUpsertPaymentSpecReqData, IHasIntId, IRequest<ErrorOr<Updated>>
{
    public UpdatePaymentSpecReq() : base(string.Empty, string.Empty, null) { }

    public UpdatePaymentSpecReq(int id, string name, string currencyCode, string? tags) :
        base(name, currencyCode, tags) =>
        Id = id;

    public int Id { get; set; }
}