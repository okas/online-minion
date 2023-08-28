using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Common;
using OnlineMinion.Contracts.Transactions.Responses;

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

    public static implicit operator UpdateTransactionCreditReq(TransactionCreditResp resp) =>
        new(resp.Id, resp.Date, resp.Amount, resp.Subject, resp.Party, resp.PaymentInstrumentId, resp.Tags);

    public static explicit operator TransactionCreditResp(UpdateTransactionCreditReq rq) =>
        new(rq.Id, rq.Date, rq.Amount, rq.Subject, rq.Party, rq.PaymentInstrumentId, rq.Tags, default);
}
