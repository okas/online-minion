using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.Transactions.Debit.Requests;

public sealed record DeleteTransactionDebitReq([Required] int Id) : IDeleteByIdCommand;
