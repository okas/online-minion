using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.Transactions.Debit.Requests;

public readonly record struct GetTransactionDebitPagingMetaInfoReq(int PageSize = 10) : IGetPagingInfoRequest;
