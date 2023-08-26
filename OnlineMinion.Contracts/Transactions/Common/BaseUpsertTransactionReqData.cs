namespace OnlineMinion.Contracts.Transactions.Common;

public abstract class BaseUpsertTransactionReqData
{
    // Create constructor all properties
    protected BaseUpsertTransactionReqData(
        DateOnly date,
        decimal  amount,
        string   subject,
        string   party,
        int      paymentInstrumentId,
        string?  tags
    )
    {
        Date = date;
        Amount = amount;
        Subject = subject;
        Party = party;
        PaymentInstrumentId = paymentInstrumentId;
        Tags = tags;
    }

    public required DateOnly Date { get; set; }

    public required decimal Amount { get; set; }

    public required string Subject { get; set; }

    public required string Party { get; set; }

    public required int PaymentInstrumentId { get; set; }

    public string? Tags { get; set; }
}
