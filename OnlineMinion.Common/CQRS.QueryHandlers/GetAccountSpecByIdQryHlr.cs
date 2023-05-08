using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.Queries;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;

namespace OnlineMinion.Common.CQRS.QueryHandlers;

public sealed class GetAccountSpecByIdQryHlr : IRequestHandler<GetAccountSpecByIdQry, AccountSpecResp?>
{
    private readonly IQueryable<AccountSpec> _queryable;

    public GetAccountSpecByIdQryHlr(OnlineMinionDbContext dbContext) =>
        _queryable = dbContext.AccountSpecs.AsNoTracking();

    public async Task<AccountSpecResp?> Handle(GetAccountSpecByIdQry rq, CancellationToken ct)
    {
        if (await _queryable.FirstOrDefaultAsync(a => a.Id == rq.Id, ct) is not { } entity)
        {
            return null;
        }

        return new()
        {
            Id = entity.Id,
            Group = entity.Group,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}
