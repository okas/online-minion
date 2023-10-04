using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;

public sealed class UpdatePaymentSpecCashReq(Guid id, string name, string? tags)
    : BaseUpsertPaymentSpecReqData(name, tags), IUpdateCommand
{
    public UpdatePaymentSpecCashReq() : this(Guid.Empty, string.Empty, null) { }

    public Guid Id { get; set; } = id;

    public static implicit operator UpdatePaymentSpecCashReq(PaymentSpecResp resp) =>
        new(resp.Id, resp.Name, resp.Tags);

    public static explicit operator PaymentSpecResp(UpdatePaymentSpecCashReq rq) =>
        new(rq.Id, rq.Name, string.Empty, rq.Tags);
}
