using OnlineMinion.Application.Contracts.PaymentSpec.Responses;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpec.Requests;

public sealed class UpdatePaymentSpecReq(Guid id, string name, string currencyCode, string? tags)
    : BaseUpsertPaymentSpecReqData(name, currencyCode, tags), IUpdateCommand
{
    public UpdatePaymentSpecReq() : this(Guid.Empty, string.Empty, string.Empty, null) { }

    public Guid Id { get; set; } = id;

    public static implicit operator UpdatePaymentSpecReq(PaymentSpecResp resp) =>
        new(resp.Id, resp.Name, resp.CurrencyCode, resp.Tags);

    public static explicit operator PaymentSpecResp(UpdatePaymentSpecReq rq) =>
        new(rq.Id, rq.Name, rq.CurrencyCode, rq.Tags);
}
