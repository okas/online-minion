using OnlineMinion.Data.BaseEntities;

namespace OnlineMinion.Data.Entities;

public class CryptoAccountSpec : BasePaymentSpec
{
    public required string ExchangeName { get; set; }

    public required bool IsFiat { get; set; }
}
