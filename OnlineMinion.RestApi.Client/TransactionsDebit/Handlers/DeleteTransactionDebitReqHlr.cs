using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Debit.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.TransactionsDebit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionDebitReqHlr(ApiProvider api)
    : BaseDeleteModelReqHlr<DeleteTransactionDebitReq>(api.Client, ApiProvider.ApiTransactionsDebitUri);
