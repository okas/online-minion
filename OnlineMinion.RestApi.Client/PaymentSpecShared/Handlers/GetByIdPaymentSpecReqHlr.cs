using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpecShared.Handlers;

[UsedImplicitly]
internal sealed class GetByIdPaymentSpecReqHlr(ApiProvider api)
    : BaseGetModelByIdReqHlr<GetByIdPaymentSpecReq, PaymentSpecResp>(api.Client, ApiProvider.ApiPaymentSpecsUri);
