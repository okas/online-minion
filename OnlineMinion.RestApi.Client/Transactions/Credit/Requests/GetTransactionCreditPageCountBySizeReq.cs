using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.RestApi.Client.Transactions.Credit.Requests;

public readonly record struct GetTransactionCreditPageCountBySizeReq(int PageSize) : IGetPagingInfoReq;
