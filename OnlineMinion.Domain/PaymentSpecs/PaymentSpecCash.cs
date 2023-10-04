namespace OnlineMinion.Domain.PaymentSpecs;

public sealed class PaymentSpecCash : BasePaymentSpecData
{
    // ReSharper disable once UnusedMember.Local
    private PaymentSpecCash() { }

    public PaymentSpecCash(string name, string currencyCode, string? tags) : base(name, currencyCode, tags) { }

    public new void Update(string name, string? tags) => base.Update(name, tags);
}
