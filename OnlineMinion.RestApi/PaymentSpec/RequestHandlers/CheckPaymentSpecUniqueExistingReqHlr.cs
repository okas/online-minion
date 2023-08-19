using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.PaymentSpec.RequestHandlers;

internal sealed class CheckPaymentSpecUniqueExistingReqHlr : IRequestHandler<CheckPaymentSpecUniqueExistingReq, bool>
{
    private readonly OnlineMinionDbContext _dbContext;
    public CheckPaymentSpecUniqueExistingReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> Handle(CheckPaymentSpecUniqueExistingReq req, CancellationToken ct) =>
        !await _dbContext.PaymentSpecs
            .AnyAsync(entity => entity.Name == req.Name && entity.Id != req.ExceptId, ct)
            .ConfigureAwait(false);
}
