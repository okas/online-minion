using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.CurrencyInfo.Requests;
using OnlineMinion.Application.Contracts.CurrencyInfo.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.CurrencyInfo.Handlers;

[UsedImplicitly]
internal class GetCurrenciesReqHlr(ApiProvider api)
    : BaseGetSomeModelsReqHlr<GetCurrenciesReq, CurrencyInfoResp>(api.Client, ApiCurrencyInfoUri);
