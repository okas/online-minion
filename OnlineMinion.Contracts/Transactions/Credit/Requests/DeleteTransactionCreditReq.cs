using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.Transactions.Credit.Requests;

public sealed record DeleteTransactionCreditReq([Required] int Id) : IDeleteByIdCommand;
