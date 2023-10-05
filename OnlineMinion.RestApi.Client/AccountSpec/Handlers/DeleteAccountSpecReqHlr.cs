using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class DeleteAccountSpecReqHlr(ApiProvider api)
    : BaseDeleteModelReqHlr<DeleteAccountSpecReq>(api.Client, ApiAccountSpecsUri);
