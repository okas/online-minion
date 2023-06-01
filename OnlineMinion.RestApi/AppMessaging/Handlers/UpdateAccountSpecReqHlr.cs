using FluentResults;
using MediatR;
using OnlineMinion.Common.Errors;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class UpdateAccountSpecReqHlr : IRequestHandler<UpdateAccountSpecReq, Result>
{
    private readonly OnlineMinionDbContext _dbContext;

    public UpdateAccountSpecReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<Result> Handle(UpdateAccountSpecReq rq, CancellationToken ct)
    {
        // TODO Handle validation and/or exception logic here

        var entity = await _dbContext.AccountSpecs.FindAsync(new object?[] { rq.Id, }, ct)
            .ConfigureAwait(false);

        if (entity is not null)
        {
            entity.Name = rq.Name;
            entity.Group = rq.Group;
            entity.Description = rq.Description;
        }
        else
        {
            return Result
                .Fail(new NotFoundError(nameof(AccountSpec), rq.Id))
                .Log<UpdateAccountSpecReqHlr>();
        }

        await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);

        return Result.Ok();
    }
}
