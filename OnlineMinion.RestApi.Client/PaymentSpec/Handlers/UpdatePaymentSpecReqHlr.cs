using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;
using static OnlineMinion.RestApi.Client.HttpMessageTransformers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecReqHlr : IRequestHandler<UpdatePaymentSpecReq, ErrorOr<Updated>>
{
    private readonly ApiClientProvider _api;
    private readonly ILogger<UpdatePaymentSpecReqHlr> _logger;

    public UpdatePaymentSpecReqHlr(ApiClientProvider api, ILogger<UpdatePaymentSpecReqHlr> logger)
    {
        _api = api;
        _logger = logger;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdatePaymentSpecReq rq, CancellationToken ct)
    {
        var uri = $"{_api.ApiV1PaymentSpecsUri}/{rq.Id}";
        using var message = await _api.Client.PutAsJsonAsync(uri, rq, ct).ConfigureAwait(false);

        if (message.IsSuccessStatusCode)
        {
            return default;
        }

        switch (message.StatusCode)
        {
            case HttpStatusCode.NotFound:
            {
                _logger.LogWarning("Payment specification with `Id={ModelId}` not found", rq.Id);
                return Error.NotFound();
            }
            case HttpStatusCode.Conflict:
            {
                _logger.LogWarning("Conflict error while updating Payment specification");
                return await TransformConflictHttpResponse<Updated>(message, ct).ConfigureAwait(false);
            }
            case HttpStatusCode.BadRequest:
            {
                _logger.LogWarning("Validation error while updating Payment specification");
                return await TransformBadRequestHttpResponse<Updated>(message, ct).ConfigureAwait(false);
            }
            default:
            {
                var reasonPhrase = message.ReasonPhrase ?? string.Empty;
                var description = message.Content.ToString() ?? string.Empty;

                _logger.LogError(
                    "Payment specification cannot be updated. Reason: {ReasonPhrase}. Description: {Description}",
                    reasonPhrase,
                    description
                );

                return Error.Failure(reasonPhrase, description);
            }
        }
    }
}
