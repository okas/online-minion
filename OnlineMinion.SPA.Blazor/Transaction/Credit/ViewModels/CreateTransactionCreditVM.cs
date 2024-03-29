using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;

namespace OnlineMinion.SPA.Blazor.Transaction.Credit.ViewModels;

/// <inheritdoc cref="BaseTransactionCreditUpsertVM" />
[method: SetsRequiredMembers]
public sealed class CreateTransactionCreditVM() : BaseTransactionCreditUpsertVM(
    default,
    DateTime.UtcNow,
    default,
    string.Empty,
    string.Empty,
    default
), ICreateCommand
{
    public CreateTransactionCreditReq ToCommand() =>
        new()
        {
            PaymentInstrumentId = PaymentInstrumentId,
            Date = DateOnly.FromDateTime(Date),
            Amount = Amount,
            Subject = Subject,
            Party = Party,
            Tags = Tags,
        };
}
