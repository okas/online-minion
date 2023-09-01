namespace OnlineMinion.Contracts.Transactions.Common;

public abstract class BaseUpsertTransactionReqData(
    int      paymentInstrumentId,
    DateOnly date,
    decimal  amount,
    string   subject,
    string   party,
    string?  tags
)
{
    public required int PaymentInstrumentId { get; set; } = paymentInstrumentId;

    public required DateOnly Date { get; set; } = date;

    public required decimal Amount { get; set; } = amount;

    public required string Subject { get; set; } = subject;

    public required string Party { get; set; } = party;

    public string? Tags { get; set; } = tags;
}
