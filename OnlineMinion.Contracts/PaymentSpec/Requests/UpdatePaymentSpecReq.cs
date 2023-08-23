using OnlineMinion.Contracts.Common.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public sealed class UpdatePaymentSpecReq : BaseUpsertPaymentSpecReqData, IUpdateCommand
{
    public UpdatePaymentSpecReq() : base(string.Empty, string.Empty, null) { }

    public UpdatePaymentSpecReq(int id, string name, string currencyCode, string? tags) :
        base(name, currencyCode, tags) =>
        Id = id;

    public int Id { get; set; }
}
