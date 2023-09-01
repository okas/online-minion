using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public sealed class UpdatePaymentSpecReq(int id, string name, string currencyCode, string? tags)
    : BaseUpsertPaymentSpecReqData(name, currencyCode, tags), IUpdateCommand
{
    public UpdatePaymentSpecReq() : this(0, string.Empty, string.Empty, null) { }

    public int Id { get; set; } = id;

    public static implicit operator UpdatePaymentSpecReq(PaymentSpecResp resp) =>
        new(resp.Id, resp.Name, resp.CurrencyCode, resp.Tags);

    public static explicit operator PaymentSpecResp(UpdatePaymentSpecReq rq) =>
        new(rq.Id, rq.Name, rq.CurrencyCode, rq.Tags);
}
