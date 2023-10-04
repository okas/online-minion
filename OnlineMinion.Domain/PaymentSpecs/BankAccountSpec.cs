namespace OnlineMinion.Domain.PaymentSpecs;

public sealed class BankAccountSpec : BasePaymentSpecData
{
    public required string IBAN { get; set; }

    public required string BankName { get; set; }
}
