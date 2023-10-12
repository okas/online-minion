using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.RestApi.Client.Helpers;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseUpdateModelReqHlr<TRequest>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<TRequest, Updated>
    where TRequest : IUpdateCommand
{
    public async Task<ErrorOr<Updated>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        using var message = await apiClient.PutAsJsonAsync(uri, rq, ct).ConfigureAwait(false);

        return await HandleFailResponse(message, ct).ConfigureAwait(false);
    }

    public virtual Uri BuildUri(TRequest rq) => new(
        $"{resource}/{rq.Id}",
        UriKind.RelativeOrAbsolute
    );

    private static async Task<ErrorOr<Updated>> HandleFailResponse(
        HttpResponseMessage response,
        CancellationToken   ct
    ) =>
        response.StatusCode switch
        {
            HttpStatusCode.NoContent => Result.Updated,
            HttpStatusCode.NotFound => Error.NotFound(),
            HttpStatusCode.Conflict => await response.TransformConflictHttpResponse(ct).ConfigureAwait(false),
            HttpStatusCode.BadRequest => await response.TransformBadRequestHttpResponse(ct).ConfigureAwait(false),
            _ => response.GetErrorWithReasonPhraseAndContent(),
        };
}
