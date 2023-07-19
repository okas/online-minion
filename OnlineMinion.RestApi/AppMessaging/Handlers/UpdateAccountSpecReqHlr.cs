using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

internal sealed class UpdateAccountSpecReqHlr : IRequestHandler<UpdateAccountSpecReq, ErrorOr<Updated>>
{
    private readonly OnlineMinionDbContext _dbContext;
    private readonly ILogger<UpdateAccountSpecReqHlr> _logger;

    public UpdateAccountSpecReqHlr(OnlineMinionDbContext dbContext, ILogger<UpdateAccountSpecReqHlr> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateAccountSpecReq rq, CancellationToken ct)
    {
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
            _logger.LogWarning("{ModelName} with Id {Id} not found", nameof(AccountSpec), rq.Id);

            return Error.NotFound();
        }

        return Result.Updated;
    }
}
