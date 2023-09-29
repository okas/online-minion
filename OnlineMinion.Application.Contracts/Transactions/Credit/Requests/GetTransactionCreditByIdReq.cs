using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Application.Contracts.Transactions.Credit.Responses;

namespace OnlineMinion.Application.Contracts.Transactions.Credit.Requests;

[UsedImplicitly]
public sealed record GetTransactionCreditByIdReq([Required] Guid Id) : IGetByIdRequest<TransactionCreditResp>;
