using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.PaymentSpec.RequestHandlers;

[UsedImplicitly]
internal sealed class CheckPaymentSpecUniqueExistingReqHlr
    : IRequestHandler<CheckPaymentSpecUniqueExistingReq, ErrorOr<Success>>
{
    private readonly OnlineMinionDbContext _dbContext;
    public CheckPaymentSpecUniqueExistingReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<ErrorOr<Success>> Handle(CheckPaymentSpecUniqueExistingReq rq, CancellationToken ct)
    {
        var result = await _dbContext.PaymentSpecs
            .AnyAsync(entity => entity.Name == rq.Name && entity.Id != rq.ExceptId, ct)
            .ConfigureAwait(false);

        return result ? Error.Conflict() : Result.Success;
    }
}
