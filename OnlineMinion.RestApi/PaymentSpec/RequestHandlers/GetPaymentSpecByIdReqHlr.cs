using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.PaymentSpec.RequestHandlers;

internal class GetPaymentSpecByIdReqHlr : IRequestHandler<GetPaymentSpecByIdReq, PaymentSpecResp?>
{
    private readonly OnlineMinionDbContext _dbContext;
    public GetPaymentSpecByIdReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<PaymentSpecResp?> Handle(GetPaymentSpecByIdReq rq, CancellationToken ct)
    {
        var entity = await _dbContext.PaymentSpecs.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == rq.Id, ct)
            .ConfigureAwait(false);

        if (entity is null)
        {
            return null;
        }

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            CurrencyCode = entity.CurrencyCode,
            Tags = entity.Tags,
        };
    }
}
