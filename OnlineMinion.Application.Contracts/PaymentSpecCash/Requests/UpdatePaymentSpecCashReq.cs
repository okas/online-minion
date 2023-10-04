using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;

public sealed class UpdatePaymentSpecCashReq(Guid id, string name, string? tags)
    : BaseUpsertPaymentSpecReqData(name, tags), IUpdateCommand
{
    public UpdatePaymentSpecCashReq() : this(Guid.Empty, string.Empty, null) { }

    public Guid Id { get; set; } = id;
}
