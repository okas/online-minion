using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Application.Contracts.Transactions.Debit.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.TransactionsDebit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionDebitByIdReqHlr(ApiProvider api)
    : BaseGetModelByIdReqHlr<GetTransactionDebitByIdReq, TransactionDebitResp?>(
        api.Client,
        ApiProvider.ApiTransactionsDebitUri
    );
