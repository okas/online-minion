using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Contracts.Shared.Requests;

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
    : BaseUpsertTransactionReqData(paymentInstrumentId, date, amount, subject, party, tags),
        IUpdateCommand
{
    public int Id { get; } = id;
}
