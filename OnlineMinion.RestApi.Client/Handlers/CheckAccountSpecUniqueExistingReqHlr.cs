using System.Globalization;
using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;
using static System.String;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class CheckAccountSpecUniqueExistingReqHlr : IRequestHandler<CheckAccountSpecUniqueExistingReq, bool>
{
    private readonly ApiClientProvider _api;
    private readonly ILogger<CheckAccountSpecUniqueExistingReqHlr> _logger;

    public CheckAccountSpecUniqueExistingReqHlr(
        ApiClientProvider                             api,
        ILogger<CheckAccountSpecUniqueExistingReqHlr> logger
    )
    {
        _api = api;
        _logger = logger;
    }

    public async Task<bool> Handle(CheckAccountSpecUniqueExistingReq req, CancellationToken ct)
    {
        var uri = Create(
            CultureInfo.InvariantCulture,
            $"{_api.ApiV1AccountSpecsUri}/validate-available-name/{req.Name}/except-id/{req.ExceptId}"
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
