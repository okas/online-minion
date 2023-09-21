using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionDebitByIdReqHlr(ApiProvider api)
    : BaseGetModelByIdReqHlr<GetTransactionDebitByIdReq, TransactionDebitResp?>(
        api.Client,
        api.ApiTransactionsDebitUri
    );
