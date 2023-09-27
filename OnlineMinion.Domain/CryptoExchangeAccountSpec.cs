using OnlineMinion.Domain.Shared;

namespace OnlineMinion.Domain;

public class CryptoExchangeAccountSpec : BasePaymentSpec
{
    public required string ExchangeName { get; set; }

    public required bool IsFiat { get; set; }
}
