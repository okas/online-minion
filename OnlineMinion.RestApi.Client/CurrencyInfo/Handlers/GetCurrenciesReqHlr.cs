using JetBrains.Annotations;
using OnlineMinion.Contracts.CurrencyInfo.Requests;
using OnlineMinion.Contracts.CurrencyInfo.Responses;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.CurrencyInfo.Handlers;

[UsedImplicitly]
internal class GetCurrenciesReqHlr(ApiClientProvider api)
    : BaseGetSomeModelsReqHlr<GetCurrenciesReq, CurrencyInfoResp>(api.Client, api.ApiV1CurrencyInfo);
