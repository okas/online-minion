using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.Transactions.Debit.Requests;

public readonly record struct GetTransactionDebitPagingMetaInfoReq(int PageSize = 10) : IGetPagingInfoRequest;
