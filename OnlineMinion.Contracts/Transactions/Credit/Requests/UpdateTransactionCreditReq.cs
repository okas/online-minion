using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Common;

namespace OnlineMinion.Contracts.Transactions.Credit.Requests;

public sealed class UpdateTransactionCreditReq : BaseUpsertTransactionReqData, IUpdateCommand
{
    public UpdateTransactionCreditReq(
        int      id,
        DateOnly date,
        decimal  amount,
        string   subject,
        string   party,
        int      paymentInstrumentId,
        string?  tags
    ) : base(date, amount, subject, party, paymentInstrumentId, tags)
        => Id = id;

    public int Id { get; }
}
