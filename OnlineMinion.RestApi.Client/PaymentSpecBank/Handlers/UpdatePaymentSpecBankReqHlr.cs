using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpecBank.Handlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecBankReqHlr(ApiProvider api)
    : BaseUpdateModelReqHlr<UpdatePaymentSpecBankReq>(api.Client, ApiProvider.ApiPaymentSpecsBankUri);
