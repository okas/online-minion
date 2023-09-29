using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Application.Contracts.Transactions.Debit.Responses;

namespace OnlineMinion.Application.Contracts.Transactions.Debit.Requests;

[UsedImplicitly]
public sealed record GetTransactionDebitByIdReq([Required] Guid Id) : IGetByIdRequest<TransactionDebitResp>;
