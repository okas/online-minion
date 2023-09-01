using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class GetPagedTransactionDebitsReqHlr(ApiClientProvider api)
    : BasePagedGetSomeModelsReqHlr<TransactionDebitResp>(api.Client, api.ApiV1TransactionsDebitUri);
