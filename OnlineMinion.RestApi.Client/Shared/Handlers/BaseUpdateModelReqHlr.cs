using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.RestApi.Client.Helpers;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseUpdateModelReqHlr<TRequest>(HttpClient apiClient, Uri resource, ILogger logger)
    : IApiClientRequestHandler<TRequest, Updated>
    where TRequest : IUpdateCommand
{
    protected abstract string ModelName { get; }

    public async Task<ErrorOr<Updated>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        using var message = await apiClient.PutAsJsonAsync(uri, rq, ct)
            .ConfigureAwait(false);

        return await HandleFailResponse(message, rq, ct).ConfigureAwait(false);
    }

    public virtual Uri BuildUri(TRequest rq) => new(
        $"{resource}/{rq.Id}",
        UriKind.RelativeOrAbsolute
    );

    // TODO: Move logging to MediatR pipeline
    private async Task<ErrorOr<Updated>> HandleFailResponse(
        HttpResponseMessage message,
        TRequest            rq,
        CancellationToken   ct
    )
    {
        switch (message.StatusCode)
        {
            case HttpStatusCode.NoContent:
            {
                return default;
            }
            case HttpStatusCode.NotFound:
            {
                logger.LogWarning("{ModelName} with `Id={ModelId}` not found", ModelName, rq.Id);
                return Error.NotFound();
            }
            case HttpStatusCode.Conflict:
            {
                logger.LogWarning("Conflict error while updating {ModelName}", ModelName);
                return await message.TransformConflictHttpResponse(ct).ConfigureAwait(false);
            }
            case HttpStatusCode.BadRequest:
            {
                logger.LogWarning("Validation error while updating {ModelName}", ModelName);
                return await message.TransformBadRequestHttpResponse(ct).ConfigureAwait(false);
            }
            default:
            {
                var reasonPhrase = message.ReasonPhrase ?? string.Empty;
                var description = message.Content.ToString() ?? string.Empty;

                logger.LogError(
                    "{ModelName} cannot be updated. Reason: {ReasonPhrase}. Description: {Description}",
                    ModelName,
                    reasonPhrase,
                    description
                );

                return Error.Failure(reasonPhrase, description);
            }
        }
    }
}
