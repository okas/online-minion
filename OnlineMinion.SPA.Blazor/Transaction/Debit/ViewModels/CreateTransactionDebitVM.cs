using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Requests;

namespace OnlineMinion.SPA.Blazor.Transaction.Debit.ViewModels;

/// <inheritdoc cref="BaseTransactionDebitUpsertVM" />
[method: SetsRequiredMembers]
public sealed class CreateTransactionDebitVM() : BaseTransactionDebitUpsertVM(
        default,
        default,
        default,
        DateTime.UtcNow,
        default,
        string.Empty,
        string.Empty,
        default
    ),
    ICreateCommand
{
    public CreateTransactionDebitReq ToCommand() =>
        new()
        {
            PaymentInstrumentId = PaymentInstrumentId,
            AccountSpecId = AccountSpecId,
            Date = DateOnly.FromDateTime(Date),
            Amount = Amount,
            Subject = Subject,
            Party = Party,
            Tags = Tags,
        };
}
