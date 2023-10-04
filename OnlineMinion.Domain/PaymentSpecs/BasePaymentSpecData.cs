namespace OnlineMinion.Domain.PaymentSpecs;

public abstract class BasePaymentSpecData : IEntity<PaymentSpecId>
{
    protected BasePaymentSpecData(string name, string currencyCode, string? tags = default, PaymentSpecId? id = default)
    {
        Id = id ?? new();
        Name = name;
        CurrencyCode = currencyCode;
        Tags = tags;
    }

    protected BasePaymentSpecData() { }

    public string Name { get; protected set; } = null!;

    public string CurrencyCode { get; protected set; } = null!;

    public string? Tags { get; protected set; }

    public PaymentSpecId Id { get; protected set; } = null!;

    protected void Update(string name, string? tags)
    {
        // Business rule: Id cannot be changed
        // Business rule: currency code cannot be changed
        Name = name;
        Tags = tags;
    }
}
