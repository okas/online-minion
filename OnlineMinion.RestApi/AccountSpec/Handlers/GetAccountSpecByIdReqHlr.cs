using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

internal sealed class GetAccountSpecByIdReqHlr : IRequestHandler<GetAccountSpecByIdReq, AccountSpecResp?>
{
    private readonly OnlineMinionDbContext _dbContext;
    public GetAccountSpecByIdReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<AccountSpecResp?> Handle(GetAccountSpecByIdReq rq, CancellationToken ct)
    {
        var entity = await _dbContext.AccountSpecs.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == rq.Id, ct)
            .ConfigureAwait(false);

        if (entity is null)
        {
            return null;
        }

        return new()
        {
            Id = entity.Id,
            Group = entity.Group,
            Name = entity.Name,
            Description = entity.Description,
        };
    }
}
