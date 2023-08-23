using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckAccountSpecUniqueExistingReqHlr
    : IRequestHandler<CheckAccountSpecUniqueExistingReq, ErrorOr<Success>>
{
    private readonly OnlineMinionDbContext _dbContext;
    public CheckAccountSpecUniqueExistingReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<ErrorOr<Success>> Handle(CheckAccountSpecUniqueExistingReq rq, CancellationToken ct)
    {
        var result = await _dbContext.AccountSpecs
            .AnyAsync(entity => entity.Name == rq.Name && entity.Id != rq.ExceptId, ct)
            .ConfigureAwait(false);

        return result ? Error.Conflict() : Result.Success;
    }
}
