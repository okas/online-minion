namespace OnlineMinion.Domain.PaymentSpecs;

public sealed class CashAccountSpec : BasePaymentSpecData
{
    // ReSharper disable once UnusedMember.Local
    private CashAccountSpec() { }

    public CashAccountSpec(string name, string currencyCode, string? tags) : base(name, currencyCode, tags) { }

    public void Update(string name, string? tags)
    {
        // Business rule: Id cannot be changed
        // Business rule: currency code cannot be changed
        Name = name;
        Tags = tags;
    }
}
