using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.Transactions.Credit.Requests;

[method: SetsRequiredMembers]
public sealed class UpdateTransactionCreditReq(
        Guid     id,
        DateOnly date,
        decimal  amount,
        string   subject,
        string   party,
        Guid     paymentInstrumentId,
        string?  tags
    )
    : BaseUpsertTransactionReqData(paymentInstrumentId, date, amount, subject, party, tags),
        IUpdateCommand
{
    public Guid Id { get; } = id;
}
