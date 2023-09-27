using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class DeleteAccountSpecReqHlr(OnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeleteAccountSpecReq, Domain.AccountSpec>(dbContext);
