using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class UpdateTransactionDebitReqHlr(
        ApiClientProvider                     api,
        ILogger<UpdateTransactionDebitReqHlr> logger
    )
    : BaseUpdateModelReqHlr<UpdateTransactionDebitReq>(api.Client, api.ApiV1TransactionsDebitUri, logger)
{
    protected override string ModelName => "Debit Transaction";
}
