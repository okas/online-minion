using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.TransactionsCredit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionCreditByIdReqHlr(ApiProvider api)
    : BaseGetModelByIdReqHlr<GetTransactionCreditByIdReq, TransactionCreditResp?>(
        api.Client,
        ApiProvider.ApiTransactionsCreditUri
    );
