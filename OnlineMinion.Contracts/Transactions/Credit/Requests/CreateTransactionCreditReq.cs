using System.Diagnostics.CodeAnalysis;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Common;

namespace OnlineMinion.Contracts.Transactions.Credit.Requests;

[method: SetsRequiredMembers]
public sealed class CreateTransactionCreditReq() : BaseUpsertTransactionReqData(
    DateOnly.FromDateTime(DateTime.Now),
    0m,
    string.Empty,
    string.Empty,
    0,
    default
), ICreateCommand;
