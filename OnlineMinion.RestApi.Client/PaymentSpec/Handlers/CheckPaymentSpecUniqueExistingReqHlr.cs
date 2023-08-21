using System.Globalization;
using System.Net;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckPaymentSpecUniqueExistingReqHlr : IRequestHandler<CheckPaymentSpecUniqueExistingReq, bool>
{
    private readonly ApiClientProvider _api;
    private readonly ILogger<CheckPaymentSpecUniqueExistingReqHlr> _logger;

    public CheckPaymentSpecUniqueExistingReqHlr(
        ApiClientProvider                             api,
        ILogger<CheckPaymentSpecUniqueExistingReqHlr> logger
    )
    {
        _api = api;
        _logger = logger;
    }

    public async Task<bool> Handle(CheckPaymentSpecUniqueExistingReq req, CancellationToken ct)
    {
        var uri = string.Create(
            CultureInfo.InvariantCulture,
            $"{_api.ApiV1PaymentSpecsUri}/validate-available-name/{req.Name}/except-id/{req.ExceptId}"
        );

        var message = new HttpRequestMessage(HttpMethod.Head, uri);

        var result = await _api.Client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        switch (result.StatusCode)
        {
            case HttpStatusCode.NoContent:
                return true;
            case HttpStatusCode.Conflict:
                return false;
            default:
                _logger.LogError("Unexpected status code {StatusCode} for {Uri}", result.StatusCode, uri);
                return false;
        }
    }
}
