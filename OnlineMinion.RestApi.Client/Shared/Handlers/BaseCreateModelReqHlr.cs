using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Application.Contracts.Shared.Responses;
using OnlineMinion.RestApi.Client.Helpers;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseCreateModelReqHlr<TRequest>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<TRequest, ModelIdResp>
    where TRequest : ICreateCommand
{
    public async Task<ErrorOr<ModelIdResp>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        using var responseMessage = await apiClient.PostAsJsonAsync(uri, rq, ct).ConfigureAwait(false);

        return await HandleResponse(responseMessage, ct).ConfigureAwait(false);
    }

    public virtual Uri BuildUri(TRequest rq) => resource;

    private static async ValueTask<ErrorOr<ModelIdResp>> HandleResponse(
        HttpResponseMessage response,
        CancellationToken   ct
    ) =>
        response.StatusCode switch
        {
            HttpStatusCode.Created => await HandleExpectedHttpResult(response, ct).ConfigureAwait(false),
            HttpStatusCode.Conflict => await response.TransformConflictHttpResponse(ct).ConfigureAwait(false),
            HttpStatusCode.BadRequest => await response.TransformBadRequestHttpResponse(ct).ConfigureAwait(false),
            _ => response.GetErrorWithReasonPhraseAndContent(),
        };

    private static async ValueTask<ErrorOr<ModelIdResp>> HandleExpectedHttpResult(
        HttpResponseMessage response,
        CancellationToken   ct
    )
    {
        var result = await response.Content.ReadFromJsonAsync<ModelIdResp>(ct).ConfigureAwait(false);

        if (result is null)
        {
            return Error.Unexpected(description: "Empty response, expected new object's Id");
        }

        if (result.Id == Guid.Empty)
        {
            return Error.Validation(description: $"Invalid response, Id is not set {result?.Id}");
        }

        return result;
    }
}
