using System.Globalization;
using System.Net;
using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckPaymentSpecUniqueExistingReqHlr
    : IRequestHandler<CheckPaymentSpecUniqueExistingReq, ErrorOr<Success>>
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

    public async Task<ErrorOr<Success>> Handle(CheckPaymentSpecUniqueExistingReq rq, CancellationToken ct)
    {
        var uri = string.Create(
            CultureInfo.InvariantCulture,
            $"{_api.ApiV1PaymentSpecsUri}/validate-available-name/{rq.Name}/except-id/{rq.ExceptId}"
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
