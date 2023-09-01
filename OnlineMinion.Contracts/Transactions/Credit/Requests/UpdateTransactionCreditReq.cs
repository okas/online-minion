using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Common;

namespace OnlineMinion.Contracts.Transactions.Credit.Requests;

[method: SetsRequiredMembers]
public sealed class UpdateTransactionCreditReq(
        int      id,
        DateOnly date,
        decimal  amount,
        string   subject,
        string   party,
        int      paymentInstrumentId,
        string?  tags
    )
    : BaseUpsertTransactionReqData(date, amount, subject, party, paymentInstrumentId, tags), IUpdateCommand
{
    public int Id => id;
}
