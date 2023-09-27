using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPagedAccountSpecsReqHlr(ApiProvider api)
    : BaseGetSomeModelsPagedReqHlr<AccountSpecResp>(api.Client, ApiProvider.ApiAccountSpecsUri);
