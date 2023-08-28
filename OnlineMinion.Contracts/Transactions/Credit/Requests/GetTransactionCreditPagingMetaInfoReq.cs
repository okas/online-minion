using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.Transactions.Credit.Requests;

public readonly record struct GetTransactionCreditPagingMetaInfoReq(int PageSize = 10) : IGetPagingInfoRequest;
