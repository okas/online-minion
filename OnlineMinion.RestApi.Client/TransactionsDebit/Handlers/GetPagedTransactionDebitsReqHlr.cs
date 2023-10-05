using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Debit.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.TransactionsDebit.Handlers;

[UsedImplicitly]
internal sealed class GetPagedTransactionDebitsReqHlr(ApiProvider api)
    : BaseGetSomeModelsPagedReqHlr<TransactionDebitResp>(api.Client, ApiTransactionsDebitUri);
