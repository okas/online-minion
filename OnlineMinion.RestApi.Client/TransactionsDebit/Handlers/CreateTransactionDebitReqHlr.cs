using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.TransactionsDebit.Handlers;

[UsedImplicitly]
internal sealed class CreateTransactionDebitReqHlr(ApiProvider api, ILogger<CreateTransactionDebitReqHlr> logger)
    : BaseCreateModelReqHlr<CreateTransactionDebitReq>(api.Client, ApiProvider.ApiTransactionsDebitUri, logger)
{
    protected override string ModelName => "Debit Transaction";
}
