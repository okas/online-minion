using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.PaymentSpecCash.Handlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecCashReqHlr(ApiProvider api)
    : BaseCreateModelReqHlr<CreatePaymentSpecCashReq>(api.Client, ApiPaymentSpecsCashUri);
