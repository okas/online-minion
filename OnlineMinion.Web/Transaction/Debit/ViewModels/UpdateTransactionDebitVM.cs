using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Requests;

namespace OnlineMinion.Web.Transaction.Debit.ViewModels;

/// <inheritdoc cref="BaseTransactionDebitUpsertVM" />
[method: SetsRequiredMembers]
public sealed class UpdateTransactionDebitVM(
    int      id,
    int      paymentInstrumentId,
    int      accountSpecId,
    decimal  fee,
    DateTime date,
    decimal  amount,
    string   subject,
    string   party,
    string?  tags
) : BaseTransactionDebitUpsertVM(paymentInstrumentId, accountSpecId, fee, date, amount, subject, party, tags),
    IUpdateCommand
{
    public int Id { get; } = id;

    public UpdateTransactionDebitReq ToCommand() => new(
        Id,
        PaymentInstrumentId,
        AccountSpecId,
        Fee,
        DateOnly.FromDateTime(Date),
        Amount,
        Subject,
        Party,
        Tags
    );
}
