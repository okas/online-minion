using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class DeletePaymentSpecReqHlr(ApiProvider api)
    : BaseDeleteModelReqHlr<DeletePaymentSpecReq>(api.Client, ApiProvider.ApiPaymentSpecsUri);
