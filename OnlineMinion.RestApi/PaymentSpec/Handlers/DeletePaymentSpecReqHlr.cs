using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class DeletePaymentSpecReqHlr : IRequestHandler<DeletePaymentSpecReq, ErrorOr<Deleted>>
{
    private readonly OnlineMinionDbContext _dbContext;
    public DeletePaymentSpecReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<ErrorOr<Deleted>> Handle(DeletePaymentSpecReq rq, CancellationToken ct)
    {
        var deletedCount = await _dbContext.PaymentSpecs
            .Where(a => a.Id == rq.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        return deletedCount > 0 ? Result.Deleted : Error.NotFound();
    }
}
