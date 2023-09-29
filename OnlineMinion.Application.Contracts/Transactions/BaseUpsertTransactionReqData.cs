namespace OnlineMinion.Application.Contracts.Transactions;

public abstract class BaseUpsertTransactionReqData(
    Guid     paymentInstrumentId,
    DateOnly date,
    decimal  amount,
    string   subject,
    string   party,
    string?  tags
)
{
    public required Guid PaymentInstrumentId { get; set; } = paymentInstrumentId;

    public DateOnly Date { get; set; } = date;

    public required decimal Amount { get; set; } = amount;

    public required string Subject { get; set; } = subject;

    public required string Party { get; set; } = party;

    public string? Tags { get; set; } = tags;
}
