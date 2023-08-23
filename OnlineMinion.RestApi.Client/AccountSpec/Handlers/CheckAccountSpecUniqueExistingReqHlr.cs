using System.Globalization;
using System.Net;
using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;
using static System.String;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckAccountSpecUniqueExistingReqHlr
    : IRequestHandler<CheckAccountSpecUniqueExistingReq, ErrorOr<Success>>
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

    public async Task<ErrorOr<Success>> Handle(CheckAccountSpecUniqueExistingReq rq, CancellationToken ct)
    {
        var uri = Create(
            CultureInfo.InvariantCulture,
            $"{_api.ApiV1AccountSpecsUri}/validate-available-name/{rq.Name}/except-id/{rq.ExceptId}"
        );

        var message = new HttpRequestMessage(HttpMethod.Head, uri);

        var result = await _api.Client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        return result.StatusCode switch
        {
            HttpStatusCode.NoContent => Result.Success,
            HttpStatusCode.Conflict => Error.Conflict(),
            _ => Error.Unexpected(),
        };
    }
}
