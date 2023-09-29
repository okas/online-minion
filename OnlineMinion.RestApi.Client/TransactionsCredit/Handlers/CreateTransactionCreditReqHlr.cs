using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.TransactionsCredit.Handlers;

[UsedImplicitly]
internal sealed class CreateTransactionCreditReqHlr(ApiProvider api, ILogger<CreateTransactionCreditReqHlr> logger)
    : BaseCreateModelReqHlr<CreateTransactionCreditReq>(api.Client, ApiProvider.ApiTransactionsCreditUri, logger)
{
    protected override string ModelName => "Credit Transaction";
}
