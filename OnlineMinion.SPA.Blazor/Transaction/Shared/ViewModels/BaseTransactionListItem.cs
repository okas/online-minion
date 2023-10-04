using OnlineMinion.Application.Contracts;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;

namespace OnlineMinion.SPA.Blazor.Transaction.Shared.ViewModels;

/// <param name="PaymentInstrument">NB! It is important too keep member name, so that API communication queries work.</param>
public abstract record BaseTransactionListItem(
    Guid                      Id,
    DateOnly                  Date,
    decimal                   Amount,
    string                    Subject,
    string                    Party,
    string?                   Tags,
    PaymentSpecDescriptorResp PaymentInstrument
) : IHasId;
