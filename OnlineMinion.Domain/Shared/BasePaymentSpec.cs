namespace OnlineMinion.Domain.Shared;

public class BasePaymentSpec : BaseEntity
{
    public required string Name { get; set; }

    public required string CurrencyCode { get; set; }

    public string? Tags { get; set; }
}
