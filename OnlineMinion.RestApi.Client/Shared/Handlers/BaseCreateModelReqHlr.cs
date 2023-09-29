using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.RestApi.Client.Helpers;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseCreateModelReqHlr<TRequest>(HttpClient apiClient, Uri resource, ILogger logger)
    : IApiClientRequestHandler<TRequest, ModelIdResp>
    where TRequest : ICreateCommand
{
    protected abstract string ModelName { get; }

    public async Task<ErrorOr<ModelIdResp>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        using var responseMessage = await apiClient.PostAsJsonAsync(uri, rq, ct)
            .ConfigureAwait(false);

        return await HandleResponse(responseMessage, ct);
    }

    public virtual Uri BuildUri(TRequest rq) => resource;

    // TODO: Move logging to MediatR pipeline
    private async ValueTask<ErrorOr<ModelIdResp>> HandleResponse(HttpResponseMessage message, CancellationToken ct)
    {
        switch (message.StatusCode)
        {
            case HttpStatusCode.Created:
            {
                var result = await message.Content
                    .ReadFromJsonAsync<ModelIdResp>(ct)
                    .ConfigureAwait(false);

                return result is not null && result.Id != Guid.Empty
                    ? result
                    : Error.Validation($"Invalid message response, Id is not set {result?.Id}");
            }
            case HttpStatusCode.Conflict:
            {
                logger.LogWarning("Conflict error while updating model `{ModelName}`", ModelName);
                return await message.TransformConflictHttpResponse(ct).ConfigureAwait(false);
            }
            case HttpStatusCode.BadRequest:
            {
                logger.LogWarning("Validation error while creating model `{ModelName}`", ModelName);
                return await message.TransformBadRequestHttpResponse(ct).ConfigureAwait(false);
            }
            default:
            {
                var reasonPhrase = message.ReasonPhrase ?? string.Empty;
                var description = message.Content.ToString() ?? string.Empty;

                logger.LogError(
                    "Model `{ModelName}` cannot be created. Reason: {ReasonPhrase}. Description: {Description}",
                    ModelName,
                    reasonPhrase,
                    description
                );

                return Error.Failure(reasonPhrase, description);
            }
        }
    }
}
