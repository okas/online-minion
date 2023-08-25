using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseCreateModelReqHlr<TRequest> : IRequestHandler<TRequest, ErrorOr<ModelIdResp>>
    where TRequest : ICreateCommand
{
    private readonly HttpClient _apiClient;
    private readonly ILogger<BaseCreateModelReqHlr<TRequest>> _logger;

    protected BaseCreateModelReqHlr(HttpClient apiClient, ILogger<BaseCreateModelReqHlr<TRequest>> logger)
        => (_apiClient, _logger) = (apiClient, logger);

    protected abstract string ModelName { get; }

    public async Task<ErrorOr<ModelIdResp>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri();

        using var responseMessage = await _apiClient.PostAsJsonAsync(uri, rq, ct)
            .ConfigureAwait(false);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return await HandleFailResponse(responseMessage, ct).ConfigureAwait(false);
        }

        var result = await responseMessage.Content
            .ReadFromJsonAsync<ModelIdResp>(ct)
            .ConfigureAwait(false);

        return result is { Id: > 0, }
            ? result
            : throw ExceptionHelpers.CreateForUnknownResponse(nameof(ModelIdResp));
    }

    private async Task<ErrorOr<ModelIdResp>> HandleFailResponse(HttpResponseMessage message, CancellationToken ct)
    {
        switch (message.StatusCode)
        {
            case HttpStatusCode.Conflict:
            {
                _logger.LogWarning("Conflict error while updating model `{ModelName}`", ModelName);
                return await message.TransformConflictHttpResponse(ct).ConfigureAwait(false);
            }
            case HttpStatusCode.BadRequest:
            {
                _logger.LogWarning("Validation error while creating model `{ModelName}`", ModelName);
                return await message.TransformBadRequestHttpResponse(ct).ConfigureAwait(false);
            }
            default:
            {
                var reasonPhrase = message.ReasonPhrase ?? string.Empty;
                var description = message.Content.ToString() ?? string.Empty;

                _logger.LogError(
                    "Model `{ModelName}` cannot be created. Reason: {ReasonPhrase}. Description: {Description}",
                    ModelName,
                    reasonPhrase,
                    description
                );

                return Error.Failure(reasonPhrase, description);
            }
        }
    }

    protected abstract Uri BuildUri();
}
