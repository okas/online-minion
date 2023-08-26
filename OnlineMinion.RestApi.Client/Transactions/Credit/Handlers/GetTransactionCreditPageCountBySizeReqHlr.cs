using JetBrains.Annotations;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using OnlineMinion.RestApi.Client.Transactions.Credit.Requests;

namespace OnlineMinion.RestApi.Client.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionCreditPageCountBySizeReqHlr(ApiClientProvider api)
    : BaseGetModelPageCountReqHlr<GetTransactionCreditPageCountBySizeReq>(api.Client, api.ApiV1TransactionsCreditUri);
