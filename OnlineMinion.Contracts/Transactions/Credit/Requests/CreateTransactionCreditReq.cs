using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.Transactions.Credit.Requests;

[method: SetsRequiredMembers]
public sealed class CreateTransactionCreditReq() : BaseUpsertTransactionReqData(
    default,
    DateOnly.FromDateTime(DateTime.Now),
    default,
    string.Empty,
    string.Empty,
    default
), ICreateCommand;
