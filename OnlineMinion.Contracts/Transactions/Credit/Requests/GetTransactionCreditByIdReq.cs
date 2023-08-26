using System.ComponentModel.DataAnnotations;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Transactions.Responses;

namespace OnlineMinion.Contracts.Transactions.Credit.Requests;

public sealed record GetTransactionCreditByIdReq([Required] int Id) : IGetByIdRequest<TransactionCreditResp?>;
