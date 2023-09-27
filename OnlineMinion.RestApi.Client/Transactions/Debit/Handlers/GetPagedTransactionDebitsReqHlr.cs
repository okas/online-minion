using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class GetPagedTransactionDebitsReqHlr(ApiProvider api)
    : BaseGetSomeModelsPagedReqHlr<TransactionDebitResp>(api.Client, ApiProvider.ApiTransactionsDebitUri);
