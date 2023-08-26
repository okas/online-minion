using System.Linq.Expressions;
using ErrorOr;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecsReqHlr :
    BaseGetSomeModelQueryHandler<BaseGetSomeModelsPagedReq<PaymentSpecResp>, PaymentSpecResp>,
    IApiRequestHandler<BaseGetSomeModelsPagedReq<PaymentSpecResp>, PagedResult<PaymentSpecResp>>
{
    public GetPaymentSpecsReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    public async Task<ErrorOr<PagedResult<PaymentSpecResp>>> Handle(
        BaseGetSomeModelsPagedReq<PaymentSpecResp> rq,
        CancellationToken                          ct
    )
    {
        var query = DbContext.PaymentSpecs.AsNoTracking();

        Expression<Func<BasePaymentSpec, PaymentSpecResp>> projection = e => new(e.Id, e.Name, e.CurrencyCode, e.Tags);

        return await GetDataFromStoreAsync(query, rq, projection, ct).ConfigureAwait(false);
    }
}
