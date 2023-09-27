using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;

namespace OnlineMinion.Contracts.Transactions.Debit.Requests;

public sealed record GetTransactionDebitByIdReq([Required] int Id) : IGetByIdRequest<TransactionDebitResp>;
