using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecReqHlr : IRequestHandler<CreatePaymentSpecReq, ErrorOr<ModelIdResp>>
{
    private readonly OnlineMinionDbContext _dbContext;
    public CreatePaymentSpecReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<ErrorOr<ModelIdResp>> Handle(CreatePaymentSpecReq rq, CancellationToken ct)
    {
        var entry = await _dbContext.PaymentSpecs.AddAsync(
                new()
                {
                    Name = rq.Name, CurrencyCode = rq.CurrencyCode, Tags = rq.Tags,
                },
                ct
            )
            .ConfigureAwait(false);

        return new ModelIdResp(entry.Entity.Id);
    }
}
