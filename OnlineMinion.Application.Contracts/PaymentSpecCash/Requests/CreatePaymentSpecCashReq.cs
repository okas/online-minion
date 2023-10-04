using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;

public sealed class CreatePaymentSpecCashReq(string name, string currencyCode, string? tags)
    : BaseUpsertPaymentSpecReqData(name, tags), ICreateCommand
{
    public CreatePaymentSpecCashReq() : this(string.Empty, string.Empty, null) { }

    public string CurrencyCode { get; set; } = currencyCode;
}
