namespace OnlineMinion.Data.BaseEntities;

public class BasePaymentSpec : BaseEntity
{
    public required string Name { get; set; }

    public required string CurrencyIso { get; set; }

    public string? Tags { get; set; }
}
