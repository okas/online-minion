using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;

namespace OnlineMinion.Contracts.Transactions.Debit.Requests;

[UsedImplicitly]
public sealed record GetTransactionDebitByIdReq([Required] Guid Id) : IGetByIdRequest<TransactionDebitResp>;
