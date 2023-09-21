using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionDebitPagingMetaInfoReqHlr(ApiProvider api)
    : BaseGetModelPagingMetaInfoReqHlr<GetTransactionDebitPagingMetaInfoReq>(
        api.Client,
        api.ApiTransactionsDebitUri
    );
