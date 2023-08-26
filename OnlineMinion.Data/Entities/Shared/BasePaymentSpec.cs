namespace OnlineMinion.Data.Entities.Shared;

public class BasePaymentSpec : BaseEntity
{
    public required string Name { get; set; }

    public required string CurrencyCode { get; set; }

    public string? Tags { get; set; }
}
