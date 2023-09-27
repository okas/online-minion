using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;

namespace OnlineMinion.Contracts.Transactions.Credit.Requests;

public sealed record GetTransactionCreditByIdReq([Required] Guid Id) : IGetByIdRequest<TransactionCreditResp>;
