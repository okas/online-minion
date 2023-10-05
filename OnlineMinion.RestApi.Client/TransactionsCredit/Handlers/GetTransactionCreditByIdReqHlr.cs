using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Application.Contracts.Transactions.Credit.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.TransactionsCredit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionCreditByIdReqHlr(ApiProvider api)
    : BaseGetModelByIdReqHlr<GetTransactionCreditByIdReq, TransactionCreditResp>(api.Client, ApiTransactionsCreditUri);
