using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Requests;

namespace OnlineMinion.Web.Transaction.Credit.ViewModels;

/// <inheritdoc cref="BaseTransactionCreditUpsertVM" />
[method: SetsRequiredMembers]
public sealed class UpdateTransactionCreditVM(
        int      id,
        int      paymentInstrumentId,
        DateTime date,
        decimal  amount,
        string   subject,
        string   party,
        string?  tags
    )
    : BaseTransactionCreditUpsertVM(paymentInstrumentId, date, amount, subject, party, tags), IUpdateCommand
{
    public int Id { get; } = id;

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
