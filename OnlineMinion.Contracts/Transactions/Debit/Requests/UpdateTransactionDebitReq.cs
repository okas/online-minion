using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.Transactions.Debit.Requests;

[method: SetsRequiredMembers]
public sealed class UpdateTransactionDebitReq(
        Guid     paymentInstrumentId,
        Guid     id,
        Guid     accountSpecId,
        decimal  fee,
        DateOnly date,
        decimal  amount,
        string   subject,
        string   party,
        string?  tags
    )
    : BaseUpsertTransactionDebitReqData(paymentInstrumentId, accountSpecId, fee, date, amount, subject, party, tags),
        IUpdateCommand
{
    public Guid Id { get; } = id;
}
