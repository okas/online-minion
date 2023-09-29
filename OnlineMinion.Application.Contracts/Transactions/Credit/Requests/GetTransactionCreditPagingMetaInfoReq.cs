using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.Transactions.Credit.Requests;

public readonly record struct GetTransactionCreditPagingMetaInfoReq(int PageSize = 10) : IGetPagingInfoRequest;
