namespace OnlineMinion.Application.Contracts.PaymentSpecCrypto;

public interface IUpsertPaymentSpecCryptoReq
{
    string ExchangeName { get; set; }

    bool IsFiat { get; set; }
}
