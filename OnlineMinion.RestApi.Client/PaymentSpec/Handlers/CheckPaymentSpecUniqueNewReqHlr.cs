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
internal sealed class CheckPaymentSpecUniqueNewReqHlr : IRequestHandler<CheckPaymentSpecUniqueNewReq, ErrorOr<Success>>
{
    private readonly ApiClientProvider _api;
    private readonly ILogger<CheckPaymentSpecUniqueNewReqHlr> _logger;

    public CheckPaymentSpecUniqueNewReqHlr(ApiClientProvider api, ILogger<CheckPaymentSpecUniqueNewReqHlr> logger)
    {
        _api = api;
        _logger = logger;
    }

    public async Task<ErrorOr<Success>> Handle(CheckPaymentSpecUniqueNewReq rq, CancellationToken ct)
    {
        var uri = string.Create(
            CultureInfo.InvariantCulture,
            $"{_api.ApiV1PaymentSpecsUri}/validate-available-name/{rq.Name}"
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
