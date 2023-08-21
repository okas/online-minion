using ErrorOr;
using MediatR;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

internal sealed class CreateAccountSpecReqHlr : IRequestHandler<CreateAccountSpecReq, ErrorOr<ModelIdResp>>
{
    private readonly OnlineMinionDbContext _dbContext;
    public CreateAccountSpecReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<ErrorOr<ModelIdResp>> Handle(CreateAccountSpecReq rq, CancellationToken ct)
    {
        var entry = await _dbContext.AccountSpecs.AddAsync(
                new(rq.Name, rq.Group, rq.Description),
                ct
            )
            .ConfigureAwait(false);

        return new ModelIdResp(entry.Entity.Id);
    }
}
