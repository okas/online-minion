using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.Transactions.Debit.Requests;

[method: SetsRequiredMembers]
public sealed class CreateTransactionDebitReq()
    : BaseUpsertTransactionDebitReqData(
            default,
            default,
            default,
            DateOnly.FromDateTime(DateTime.Now),
            default,
            default,
            default,
            default
        ),
        ICreateCommand;
