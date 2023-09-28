using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class DeleteAccountSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeleteAccountSpecReq, Domain.AccountSpec>(dbContext);
