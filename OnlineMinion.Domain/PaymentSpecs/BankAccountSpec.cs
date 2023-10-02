namespace OnlineMinion.Domain.PaymentSpecs;

public sealed class BankAccountSpec : BasePaymentSpec
{
    public required string IBAN { get; set; }

    public required string BankName { get; set; }
}
