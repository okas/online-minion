using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Credit.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.TransactionsCredit.Handlers;

[UsedImplicitly]
internal sealed class GetPagedTransactionCreditsReqHlr(ApiProvider api)
    : BaseGetSomeModelsPagedReqHlr<TransactionCreditResp>(api.Client, ApiTransactionsCreditUri);
