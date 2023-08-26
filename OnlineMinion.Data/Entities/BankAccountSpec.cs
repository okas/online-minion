using OnlineMinion.Data.Entities.Shared;

namespace OnlineMinion.Data.Entities;

public class BankAccountSpec : BasePaymentSpec
{
    public required string IBAN { get; set; }

    public required string BankName { get; set; }
}
