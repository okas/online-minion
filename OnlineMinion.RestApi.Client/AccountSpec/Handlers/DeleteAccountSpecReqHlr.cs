using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class DeleteAccountSpecReqHlr(ApiProvider api)
    : BaseDeleteModelReqHlr<DeleteAccountSpecReq>(api.Client, ApiProvider.ApiAccountSpecsUri);
