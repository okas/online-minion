using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Debit.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.TransactionsDebit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionDebitPagingMetaInfoReqHlr(ApiProvider api)
    : BaseGetModelPagingMetaInfoReqHlr<GetTransactionDebitPagingMetaInfoReq>(api.Client, ApiTransactionsDebitUri);
