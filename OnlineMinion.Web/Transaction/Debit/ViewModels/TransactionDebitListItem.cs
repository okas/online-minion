using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.Web.Transaction.Common.ViewModels;

namespace OnlineMinion.Web.Transaction.Debit.ViewModels;

/// <inheritdoc />
/// <param name="AccountSpec">NB! It is important too keep member name, so that API communication queries work.</param>
public sealed record TransactionDebitListItem(
    int                       Id,
    decimal                   Fee,
    DateOnly                  Date,
    decimal                   Amount,
    string                    Subject,
    string                    Party,
    string?                   Tags,
    PaymentSpecDescriptorResp PaymentInstrument,
    AccountSpecDescriptorResp AccountSpec
) : BaseTransactionListItem(
    Id,
    Date,
    Amount,
    Subject,
    Party,
    Tags,
    PaymentInstrument
)
{
    public static TransactionDebitListItem FromResponseDto(
        TransactionDebitResp      resp,
        PaymentSpecDescriptorResp paymentDescriptor,
        AccountSpecDescriptorResp accountDescriptor
    ) =>
        new(
            resp.Id,
            resp.Fee,
            resp.Date,
            resp.Amount,
            resp.Subject,
            resp.Party,
            resp.Tags,
            paymentDescriptor,
            accountDescriptor
        );

    public static TransactionDebitListItem FromUpdateRequest(
        UpdateTransactionDebitReq rq,
        PaymentSpecDescriptorResp paymentDescriptor,
        AccountSpecDescriptorResp accountDescriptor
    ) =>
        new(
            rq.Id,
            rq.Fee,
            rq.Date,
            rq.Amount,
            rq.Subject,
            rq.Party,
            rq.Tags,
            paymentDescriptor,
            accountDescriptor
        );

    public UpdateTransactionDebitVM ToUpdateVM() =>
        new(
            Id,
            PaymentInstrument.Id,
            AccountSpec.Id,
            Fee,
            Date.ToDateTime(TimeOnly.MinValue),
            Amount,
            Subject,
            Party,
            Tags
        );
}
