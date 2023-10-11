using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecCrypto.Requests;

public sealed class CreatePaymentSpecCryptoReq(
        string  exchangeName,
        bool    isFiat,
        string  name,
        string  currencyCode,
        string? tags
    )
    : BaseUpsertPaymentSpecReqData(name, tags), ICreateCommand
{
    public CreatePaymentSpecCryptoReq()
        : this(string.Empty, false, string.Empty, string.Empty, null) { }

    public string ExchangeName { get; set; } = exchangeName;

    public bool IsFiat { get; set; } = isFiat;

    public string CurrencyCode { get; set; } = currencyCode;
}
