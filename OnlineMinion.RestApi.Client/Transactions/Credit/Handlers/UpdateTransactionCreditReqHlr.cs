using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class UpdateTransactionCreditReqHlr(
        ApiClientProvider                      api,
        ILogger<UpdateTransactionCreditReqHlr> logger
    )
    : BaseUpdateModelReqHlr<UpdateTransactionCreditReq>(api.Client, api.ApiV1TransactionsCreditUri, logger)
{
    protected override string ModelName => "Credit Transaction";
}
