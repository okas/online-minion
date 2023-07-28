using OnlineMinion.Data.BaseEntities;

namespace OnlineMinion.Data.Entities;

public class CryptoExchangeAccountSpec : BasePaymentSpec
{
    public required string ExchangeName { get; set; }

    public required bool IsFiat { get; set; }
}