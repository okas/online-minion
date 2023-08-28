using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Common;
using OnlineMinion.Contracts.Transactions.Responses;

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

    public static implicit operator UpdateTransactionCreditReq(TransactionCreditResp resp) =>
        new(resp.Id, resp.Date, resp.Amount, resp.Subject, resp.Party, resp.PaymentInstrumentId, resp.Tags);

    public static explicit operator TransactionCreditResp(UpdateTransactionCreditReq rq) =>
        new(rq.Id, rq.Date, rq.Amount, rq.Subject, rq.Party, rq.PaymentInstrumentId, rq.Tags, default);
}
