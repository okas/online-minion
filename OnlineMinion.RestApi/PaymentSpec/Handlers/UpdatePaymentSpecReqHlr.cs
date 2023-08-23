using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.Data.BaseEntities;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecReqHlr : IRequestHandler<UpdatePaymentSpecReq, ErrorOr<Updated>>
{
    private readonly OnlineMinionDbContext _dbContext;
    private readonly ILogger<UpdatePaymentSpecReqHlr> _logger;

    public UpdatePaymentSpecReqHlr(OnlineMinionDbContext dbContext, ILogger<UpdatePaymentSpecReqHlr> logger) =>
        (_dbContext, _logger) = (dbContext, logger);

    public async Task<ErrorOr<Updated>> Handle(UpdatePaymentSpecReq rq, CancellationToken ct)
    {
        var entity = await _dbContext.PaymentSpecs.FindAsync(new object?[] { rq.Id, }, ct)
            .ConfigureAwait(false);

        if (entity is not null)
        {
            entity.Name = rq.Name;
            entity.CurrencyCode = rq.CurrencyCode;
            entity.Tags = rq.Tags;
        }
        else
        {
            _logger.LogWarning("{ModelName} with Id {Id} not found", nameof(BasePaymentSpec), rq.Id);

            return Error.NotFound();
        }

        return Result.Updated;
    }
}
