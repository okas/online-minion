using System.ComponentModel.DataAnnotations;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.Transactions.Debit.Requests;

public sealed record DeleteTransactionDebitReq([Required] Guid Id) : IDeleteByIdCommand;
