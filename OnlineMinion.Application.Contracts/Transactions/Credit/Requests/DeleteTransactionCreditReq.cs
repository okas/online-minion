using System.ComponentModel.DataAnnotations;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.Transactions.Credit.Requests;

public sealed record DeleteTransactionCreditReq([Required] Guid Id) : IDeleteByIdCommand;
