namespace OnlineMinion.Domain.PaymentSpecs;

public sealed class CryptoExchangeAccountSpec : BasePaymentSpec
{
    public required string ExchangeName { get; set; }

    public required bool IsFiat { get; set; }
}
