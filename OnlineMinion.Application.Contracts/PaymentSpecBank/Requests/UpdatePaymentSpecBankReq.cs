using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;

public sealed class UpdatePaymentSpecBankReq(Guid id, string iban, string bankName, string name, string? tags)
    : BaseUpsertPaymentSpecReqData(name, tags), IUpdateCommand, IUpsertPaymentSpecBankReq
{
    public UpdatePaymentSpecBankReq() :
        this(Guid.Empty, string.Empty, string.Empty, string.Empty, null) { }

    public Guid Id { get; set; } = id;

    public string IBAN { get; set; } = iban;

    public string BankName { get; set; } = bankName;
}
