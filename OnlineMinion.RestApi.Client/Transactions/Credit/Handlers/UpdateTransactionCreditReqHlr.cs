using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class UpdateTransactionCreditReqHlr(ApiProvider api, ILogger<UpdateTransactionCreditReqHlr> logger)
    : BaseUpdateModelReqHlr<UpdateTransactionCreditReq>(api.Client, ApiProvider.ApiTransactionsCreditUri, logger)
{
    protected override string ModelName => "Credit Transaction";
}
