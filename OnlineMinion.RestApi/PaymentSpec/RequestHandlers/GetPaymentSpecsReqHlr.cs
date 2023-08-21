using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Contracts.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.RestApi.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.RequestHandlers;

internal sealed class GetPaymentSpecsReqHlr : BaseQueryHandler<BaseGetSomeReq<PaymentSpecResp>, PaymentSpecResp>,
    IRequestHandler<BaseGetSomeReq<PaymentSpecResp>, PagedResult<PaymentSpecResp>>
{
    public GetPaymentSpecsReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    public async Task<PagedResult<PaymentSpecResp>> Handle(
        BaseGetSomeReq<PaymentSpecResp> rq,
        CancellationToken               ct
    )
    {
        var query = DbContext.PaymentSpecs.AsNoTracking();

        Expression<Func<BasePaymentSpec, PaymentSpecResp>> projection = e => new(e.Id, e.Name, e.CurrencyCode, e.Tags);

        return await GetDataFromStoreAsync(query, rq, projection, ct).ConfigureAwait(false);
    }
}
