using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class DeleteAccountSpecReqHlr(ApiClientProvider api)
    : BaseDeleteModelReqHlr<DeleteAccountSpecReq>(api.Client, api.ApiV1AccountSpecsUri);
