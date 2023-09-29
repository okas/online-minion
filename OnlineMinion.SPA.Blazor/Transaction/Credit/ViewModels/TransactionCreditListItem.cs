using OnlineMinion.Application.Contracts.PaymentSpec.Responses;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Application.Contracts.Transactions.Credit.Responses;
using OnlineMinion.SPA.Blazor.Transaction.Shared.ViewModels;

namespace OnlineMinion.SPA.Blazor.Transaction.Credit.ViewModels;

/// <inheritdoc />
public sealed record TransactionCreditListItem(
    Guid                      Id,
    DateOnly                  Date,
    decimal                   Amount,
    string                    Subject,
    string                    Party,
    string?                   Tags,
    PaymentSpecDescriptorResp PaymentInstrument
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
    public static TransactionCreditListItem FromResponseDto(
        TransactionCreditResp     resp,
        PaymentSpecDescriptorResp paymentDescriptor
    ) =>
        new(
            resp.Id,
            resp.Date,
            resp.Amount,
            resp.Subject,
            resp.Party,
            resp.Tags,
            paymentDescriptor
        );

    public static TransactionCreditListItem FromUpdateRequest(
        UpdateTransactionCreditReq rq,
        PaymentSpecDescriptorResp  paymentDescriptor
    ) =>
        new(
            rq.Id,
            rq.Date,
            rq.Amount,
            rq.Subject,
            rq.Party,
            rq.Tags,
            paymentDescriptor
        );

    public UpdateTransactionCreditVM ToUpdateVM() =>
        new(
            Id,
            PaymentInstrument.Id,
            Date.ToDateTime(TimeOnly.MinValue),
            Amount,
            Subject,
            Party,
            Tags
        );
}
