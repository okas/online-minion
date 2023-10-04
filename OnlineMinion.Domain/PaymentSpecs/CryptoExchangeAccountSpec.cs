namespace OnlineMinion.Domain.PaymentSpecs;

public sealed class CryptoExchangeAccountSpec : BasePaymentSpecData
{
    public required string ExchangeName { get; set; }

    public required bool IsFiat { get; set; }
}
