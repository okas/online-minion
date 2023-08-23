using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.Common.Responses;
using OnlineMinion.RestApi.Client.Infrastructure;
using static OnlineMinion.RestApi.Client.HttpMessageTransformers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CreateAccountSpecReqHlr : IRequestHandler<CreateAccountSpecReq, ErrorOr<ModelIdResp>>
{
    private readonly ApiClientProvider _api;
    private readonly ILogger<CreateAccountSpecReqHlr> _logger;

    public CreateAccountSpecReqHlr(ApiClientProvider api, ILogger<CreateAccountSpecReqHlr> logger)
    {
        _api = api;
        _logger = logger;
    }

    public async Task<ErrorOr<ModelIdResp>> Handle(CreateAccountSpecReq request, CancellationToken ct)
    {
        using var message = await _api.Client.PostAsJsonAsync(_api.ApiV1AccountSpecsUri, request, ct)
            .ConfigureAwait(false);

        if (message.IsSuccessStatusCode)
        {
            return await message.Content.ReadFromJsonAsync<ModelIdResp>(ct).ConfigureAwait(false)
                is { Id: > 0, } modelIdResp
                ? modelIdResp
                : throw ExceptionHelpers.CreateForUnknownResponse(nameof(ModelIdResp));
        }

        switch (message.StatusCode)
        {
            case HttpStatusCode.Conflict:
            {
                _logger.LogWarning("Conflict error while updating Account specification");
                return await TransformConflictHttpResponse<ModelIdResp>(message, ct).ConfigureAwait(false);
            }
            case HttpStatusCode.BadRequest:
            {
                _logger.LogWarning("Validation error while creating Account specification");
                return await TransformBadRequestHttpResponse<ModelIdResp>(message, ct).ConfigureAwait(false);
            }
            default:
            {
                var reasonPhrase = message.ReasonPhrase ?? string.Empty;
                var description = message.Content.ToString() ?? string.Empty;

                _logger.LogError(
                    "Account specification cannot be created. Reason: {ReasonPhrase}. Description: {Description}",
                    reasonPhrase,
                    description
                );

                return Error.Failure(reasonPhrase, description);
            }
        }
    }
}
