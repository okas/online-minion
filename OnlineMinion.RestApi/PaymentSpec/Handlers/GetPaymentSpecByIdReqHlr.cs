using ErrorOr;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal class GetPaymentSpecByIdReqHlr : IApiRequestHandler<GetPaymentSpecByIdReq, PaymentSpecResp?>
{
    private readonly OnlineMinionDbContext _dbContext;
    public GetPaymentSpecByIdReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<ErrorOr<PaymentSpecResp?>> Handle(GetPaymentSpecByIdReq rq, CancellationToken ct)
    {
        var entity = await _dbContext.PaymentSpecs.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == rq.Id, ct)
            .ConfigureAwait(false);

        if (entity is null)
        {
            return default;
        }

        return new PaymentSpecResp
        {
            Id = entity.Id,
            Name = entity.Name,
            CurrencyCode = entity.CurrencyCode,
            Tags = entity.Tags,
        };
    }
}
