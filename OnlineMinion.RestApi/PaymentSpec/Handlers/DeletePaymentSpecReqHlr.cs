using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Domain.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class DeletePaymentSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeletePaymentSpecReq, BasePaymentSpec>(dbContext);
