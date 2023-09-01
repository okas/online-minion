namespace OnlineMinion.Contracts.Transactions.Debit;

public abstract class BaseUpsertTransactionDebitReqData(
        int      paymentInstrumentId,
        int      accountSpecId,
        decimal  fee,
        DateOnly date,
        decimal  amount,
        string   subject,
        string   party,
        string?  tags
    )
    : BaseUpsertTransactionReqData(paymentInstrumentId, date, amount, subject, party, tags)
{
    public int AccountSpecId { get; set; } = accountSpecId;

    public decimal Fee { get; set; } = fee;
}
