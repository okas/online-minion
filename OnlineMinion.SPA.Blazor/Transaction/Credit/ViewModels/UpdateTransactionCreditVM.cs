using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;

namespace OnlineMinion.SPA.Blazor.Transaction.Credit.ViewModels;

/// <inheritdoc cref="BaseTransactionCreditUpsertVM" />
[method: SetsRequiredMembers]
public sealed class UpdateTransactionCreditVM(
        Guid     id,
        Guid     paymentInstrumentId,
        DateTime date,
        decimal  amount,
        string   subject,
        string   party,
        string?  tags
    )
    : BaseTransactionCreditUpsertVM(paymentInstrumentId, date, amount, subject, party, tags), IUpdateCommand
{
    public Guid Id { get; } = id;

    public UpdateTransactionCreditReq ToCommand() => new(
        Id,
        DateOnly.FromDateTime(Date),
        Amount,
        Subject,
        Party,
        PaymentInstrumentId,
        Tags
    );
}
