namespace OnlineMinion.Domain.PaymentSpecs;

public sealed class CashAccountSpec : BasePaymentSpecData
{
    // ReSharper disable once UnusedMember.Local
    private CashAccountSpec() { }

    public CashAccountSpec(string name, string currencyCode, string? tags) : base(name, currencyCode, tags) { }

    public new void Update(string name, string? tags) => base.Update(name, tags);
}
