using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public sealed class UpdatePaymentSpecReq : BaseUpsertPaymentSpecReqData, IUpdateCommand
{
    public UpdatePaymentSpecReq() : base(string.Empty, string.Empty, null) { }

    public UpdatePaymentSpecReq(int id, string name, string currencyCode, string? tags) :
        base(name, currencyCode, tags) =>
        Id = id;

    public int Id { get; set; }

    public static implicit operator UpdatePaymentSpecReq(PaymentSpecResp resp) =>
        new(resp.Id, resp.Name, resp.CurrencyCode, resp.Tags);

    public static explicit operator PaymentSpecResp(UpdatePaymentSpecReq rq) =>
        new(rq.Id, rq.Name, rq.CurrencyCode, rq.Tags);
}
