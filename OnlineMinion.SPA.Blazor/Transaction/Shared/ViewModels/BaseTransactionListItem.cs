using OnlineMinion.Contracts;
using OnlineMinion.Contracts.PaymentSpec.Responses;

namespace OnlineMinion.SPA.Blazor.Transaction.Shared.ViewModels;

/// <param name="PaymentInstrument">NB! It is important too keep member name, so that API communication queries work.</param>
public abstract record BaseTransactionListItem(
    int                       Id,
    DateOnly                  Date,
    decimal                   Amount,
    string                    Subject,
    string                    Party,
    string?                   Tags,
    PaymentSpecDescriptorResp PaymentInstrument
) : IHasId;
