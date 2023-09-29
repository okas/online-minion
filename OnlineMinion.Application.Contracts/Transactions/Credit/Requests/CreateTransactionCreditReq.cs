using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.Transactions.Credit.Requests;

[method: SetsRequiredMembers]
public sealed class CreateTransactionCreditReq()
    : BaseUpsertTransactionReqData(
        default,
        DateOnly.FromDateTime(DateTime.Now),
        default,
        default,
        default,
        default
    ), ICreateCommand;
