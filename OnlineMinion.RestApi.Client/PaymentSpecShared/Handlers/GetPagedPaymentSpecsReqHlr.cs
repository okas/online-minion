using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.PaymentSpecShared.Handlers;

[UsedImplicitly]
internal sealed class GetPagedPaymentSpecsReqHlr(ApiProvider api)
    : BaseGetSomeModelsPagedReqHlr<PaymentSpecResp>(api.Client, ApiPaymentSpecsUri);
