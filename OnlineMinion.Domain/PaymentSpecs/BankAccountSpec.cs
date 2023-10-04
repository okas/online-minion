namespace OnlineMinion.Domain.PaymentSpecs;

public sealed class BankAccountSpec : BasePaymentSpecData
{
    // ReSharper disable once UnusedMember.Local
    private BankAccountSpec() { }

    public BankAccountSpec(string iban, string bankName, string name, string currencyCode, string? tags)
        : base(name, currencyCode, tags)
    {
        IBAN = iban;
        BankName = bankName;
    }

    public string IBAN { get; private set; } = null!;

    public string BankName { get; private set; } = null!;

    public void Update(string iban, string bankName, string name, string? tags)
    {
        base.Update(name, tags);
        IBAN = iban;
        BankName = bankName;
    }
}
