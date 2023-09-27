using OnlineMinion.Domain.Shared;

namespace OnlineMinion.Domain;

public class BankAccountSpec : BasePaymentSpec
{
    public required string IBAN { get; set; }

    public required string BankName { get; set; }
}
