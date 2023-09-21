using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class UpdateTransactionDebitReqHlr(ApiProvider api, ILogger<UpdateTransactionDebitReqHlr> logger)
    : BaseUpdateModelReqHlr<UpdateTransactionDebitReq>(api.Client, api.ApiTransactionsDebitUri, logger)
{
    protected override string ModelName => "Debit Transaction";
}
