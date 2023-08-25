using System.Linq.Expressions;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.Common.Requests;
using OnlineMinion.Contracts.Common.Responses;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.RestApi.Common.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecsReqHlr : BaseQueryHandler<BaseGetSomePagedReq<PaymentSpecResp>, PaymentSpecResp>,
    IRequestHandler<BaseGetSomePagedReq<PaymentSpecResp>, PagedResult<PaymentSpecResp>>
{
    public GetPaymentSpecsReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    public async Task<PagedResult<PaymentSpecResp>> Handle(
        BaseGetSomePagedReq<PaymentSpecResp> rq,
        CancellationToken                    ct
    )
    {
        var query = DbContext.PaymentSpecs.AsNoTracking();

        Expression<Func<BasePaymentSpec, PaymentSpecResp>> projection = e => new(e.Id, e.Name, e.CurrencyCode, e.Tags);

        return await GetDataFromStoreAsync(query, rq, projection, ct).ConfigureAwait(false);
    }
}
