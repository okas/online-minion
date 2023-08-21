using System.Net;
using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class DeleteAccountSpecReqHlr : IRequestHandler<DeleteAccountSpecReq, ErrorOr<Deleted>>
{
    private readonly ApiClientProvider _api;
    public DeleteAccountSpecReqHlr(ApiClientProvider api) => _api = api;

    public async Task<ErrorOr<Deleted>> Handle(DeleteAccountSpecReq request, CancellationToken cancellationToken)
    {
        var uri = $"{_api.ApiV1AccountSpecsUri}/{request.Id}";
        var responseMessage = await _api.Client.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);

        return responseMessage.StatusCode switch
        {
            HttpStatusCode.NoContent => Result.Deleted,
            HttpStatusCode.NotFound => Error.NotFound(),
            _ => Error.Unexpected(),
        };
    }
}
