using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecCrypto.Requests;

public sealed class UpdatePaymentSpecCryptoReq(Guid id, string exchangeName, bool isFiat, string name, string? tags)
    : BaseUpsertPaymentSpecReqData(name, tags), IUpdateCommand, IUpsertPaymentSpecCryptoReq
{
    public UpdatePaymentSpecCryptoReq() : this(
        Guid.Empty,
        string.Empty,
        default,
        string.Empty,
        null
    ) { }

    public Guid Id { get; set; } = id;

    public string ExchangeName { get; set; } = exchangeName;

    public bool IsFiat { get; set; } = isFiat;
}
