namespace OnlineMinion.Contracts.Transactions.Debit;

public abstract class BaseUpsertTransactionDebitReqData(
        Guid     paymentInstrumentId,
        Guid     accountSpecId,
        decimal  fee,
        DateOnly date,
        decimal  amount,
        string   subject,
        string   party,
        string?  tags
    )
    : BaseUpsertTransactionReqData(paymentInstrumentId, date, amount, subject, party, tags)
{
    public Guid AccountSpecId { get; set; } = accountSpecId;

    public decimal Fee { get; set; } = fee;
}
