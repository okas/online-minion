using System.Net;
using ErrorOr;
using MediatR;
using OnlineMinion.Contracts.Common.Requests;

namespace OnlineMinion.RestApi.Client.Common.Handlers;

internal abstract class BaseDeleteModelReqHlr<TRequest> : IRequestHandler<TRequest, ErrorOr<Deleted>>
    where TRequest : IDeleteByIdRequest
{
    private readonly HttpClient _apiClient;
    protected BaseDeleteModelReqHlr(HttpClient apiClient) => _apiClient = apiClient;

    public async Task<ErrorOr<Deleted>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUrl(rq);

        var responseMessage = await _apiClient.DeleteAsync(uri, ct).ConfigureAwait(false);

        return responseMessage.StatusCode switch
        {
            HttpStatusCode.NoContent => Result.Deleted,
            HttpStatusCode.NotFound => Error.NotFound(),
            _ => Error.Unexpected(),
        };
    }

    protected abstract Uri BuildUrl(TRequest request);
}
