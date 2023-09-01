using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionDebitReqHlr(ApiClientProvider api)
    : BaseDeleteModelReqHlr<DeleteTransactionDebitReq>(api.Client, api.ApiV1TransactionsDebitUri);
