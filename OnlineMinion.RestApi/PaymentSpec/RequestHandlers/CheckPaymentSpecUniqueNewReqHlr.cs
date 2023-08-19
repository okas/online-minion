using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.PaymentSpec.RequestHandlers;

internal sealed class CheckPaymentSpecUniqueNewReqHlr : IRequestHandler<CheckPaymentSpecUniqueNewReq, bool>
{
    private readonly OnlineMinionDbContext _dbContext;
    public CheckPaymentSpecUniqueNewReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> Handle(CheckPaymentSpecUniqueNewReq req, CancellationToken ct) =>
        await _dbContext.PaymentSpecs
            .AllAsync(entity => entity.Name != req.Name, ct)
            .ConfigureAwait(false);
}
