using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;

public sealed class CreatePaymentSpecBankReq(
        string  iban,
        string  bankName,
        string  name,
        string  currencyCode,
        string? tags
    )
    : BaseUpsertPaymentSpecReqData(name, tags), IUpsertPaymentSpecBankReq, ICreateCommand
{
    public CreatePaymentSpecBankReq()
        : this(string.Empty, string.Empty, string.Empty, string.Empty, null) { }

    public string CurrencyCode { get; set; } = currencyCode;

    public string IBAN { get; set; } = iban;

    public string BankName { get; set; } = bankName;
}
