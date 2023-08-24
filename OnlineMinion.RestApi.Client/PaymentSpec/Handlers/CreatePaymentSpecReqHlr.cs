using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.Common.Responses;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;
using static OnlineMinion.RestApi.Client.HttpMessageTransformers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecReqHlr : IRequestHandler<CreatePaymentSpecReq, ErrorOr<ModelIdResp>>
{
    private readonly ApiClientProvider _api;
    private readonly ILogger<CreatePaymentSpecReqHlr> _logger;

    public CreatePaymentSpecReqHlr(ApiClientProvider api, ILogger<CreatePaymentSpecReqHlr> logger) =>
        (_api, _logger) = (api, logger);

    public async Task<ErrorOr<ModelIdResp>> Handle(CreatePaymentSpecReq request, CancellationToken ct)
    {
        using var message = await _api.Client.PostAsJsonAsync(_api.ApiV1PaymentSpecsUri, request, ct)
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
                _logger.LogWarning("Conflict error while updating Payment specification");
                return await TransformConflictHttpResponse<ModelIdResp>(message, ct)
                    .ConfigureAwait(false);
            }
            case HttpStatusCode.BadRequest:
            {
                _logger.LogWarning("Validation error while creating Payment specification");
                return await TransformBadRequestHttpResponse<ModelIdResp>(message, ct)
                    .ConfigureAwait(false);
            }
            default:
            {
                var reasonPhrase = message.ReasonPhrase ?? string.Empty;
                var description = message.Content.ToString() ?? string.Empty;

                _logger.LogError(
                    "Payment specification cannot be created. Reason: {ReasonPhrase}. Description: {Description}",
                    reasonPhrase,
                    description
                );

                return Error.Failure(reasonPhrase, description);
            }
        }
    }
}
