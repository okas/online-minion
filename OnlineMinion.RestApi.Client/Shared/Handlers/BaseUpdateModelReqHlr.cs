using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseUpdateModelReqHlr<TRequest> : IRequestHandler<TRequest, ErrorOr<Updated>>
    where TRequest : IUpdateCommand
{
    private readonly HttpClient _apiClient;
    private readonly ILogger<BaseUpdateModelReqHlr<TRequest>> _logger;

    protected BaseUpdateModelReqHlr(HttpClient apiClient, ILogger<BaseUpdateModelReqHlr<TRequest>> logger)
        => (_apiClient, _logger) = (apiClient, logger);

    protected abstract string ModelName { get; }

    public async Task<ErrorOr<Updated>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        using var message = await _apiClient.PutAsJsonAsync(uri, rq, ct)
            .ConfigureAwait(false);

        return message.IsSuccessStatusCode
            ? default
            : await HandleFailResponse(message, rq, ct).ConfigureAwait(false);
    }

    private async Task<ErrorOr<Updated>> HandleFailResponse(
        HttpResponseMessage message,
        TRequest            rq,
        CancellationToken   ct
    )
    {
        switch (message.StatusCode)
        {
            case HttpStatusCode.NotFound:
            {
                _logger.LogWarning("{ModelName} with `Id={ModelId}` not found", ModelName, rq.Id);
                return Error.NotFound();
            }
            case HttpStatusCode.Conflict:
            {
                _logger.LogWarning("Conflict error while updating {ModelName}", ModelName);
                return await message.TransformConflictHttpResponse(ct).ConfigureAwait(false);
            }
            case HttpStatusCode.BadRequest:
            {
                _logger.LogWarning("Validation error while updating {ModelName}", ModelName);
                return await message.TransformBadRequestHttpResponse(ct).ConfigureAwait(false);
            }
            default:
            {
                var reasonPhrase = message.ReasonPhrase ?? string.Empty;
                var description = message.Content.ToString() ?? string.Empty;

                _logger.LogError(
                    "{ModelName} cannot be updated. Reason: {ReasonPhrase}. Description: {Description}",
                    ModelName,
                    reasonPhrase,
                    description
                );

                return Error.Failure(reasonPhrase, description);
            }
        }
    }

    protected abstract Uri BuildUri(TRequest rq);
}
