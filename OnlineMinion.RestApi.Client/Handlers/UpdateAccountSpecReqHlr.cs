using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;
using static OnlineMinion.RestApi.Client.HttpMessageTransformers;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class UpdateAccountSpecReqHlr : IRequestHandler<UpdateAccountSpecReq, ErrorOr<bool>>
{
    private readonly ApiClientProvider _api;
    private readonly ILogger<UpdateAccountSpecReqHlr> _logger;

    public UpdateAccountSpecReqHlr(ApiClientProvider api, ILogger<UpdateAccountSpecReqHlr> logger)
    {
        _api = api;
        _logger = logger;
    }

    public async Task<ErrorOr<bool>> Handle(UpdateAccountSpecReq request, CancellationToken ct)
    {
        var uri = $"{_api.ApiV1AccountSpecsUri}/{request.Id}";
        using var message = await _api.Client.PutAsJsonAsync(uri, request, ct).ConfigureAwait(false);

        if (message.IsSuccessStatusCode)
        {
            return default;
        }

        switch (message.StatusCode)
        {
            case HttpStatusCode.NotFound:
            {
                _logger.LogWarning("Account specification with `Id={ModelId}` not found", request.Id);
                return Error.NotFound();
            }
            case HttpStatusCode.Conflict:
            {
                _logger.LogWarning("Conflict error while updating Account specification");
                return await TransformConflictHttpResponse<bool>(message, ct).ConfigureAwait(false);
            }
            case HttpStatusCode.BadRequest:
            {
                _logger.LogWarning("Validation error while updating Account specification");
                return await TransformBadRequestHttpResponse<bool>(message, ct).ConfigureAwait(false);
            }
            default:
            {
                var reasonPhrase = message.ReasonPhrase ?? string.Empty;
                var description = message.Content.ToString() ?? string.Empty;

                _logger.LogError(
                    "Account specification cannot be updated. Reason: {ReasonPhrase}. Description: {Description}",
                    reasonPhrase,
                    description
                );

                return Error.Failure(reasonPhrase, description);
            }
        }
    }
}
