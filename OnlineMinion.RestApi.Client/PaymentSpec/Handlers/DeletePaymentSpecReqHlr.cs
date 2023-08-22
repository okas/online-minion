using System.Net;
using ErrorOr;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class DeletePaymentSpecReqHlr : IRequestHandler<DeletePaymentSpecReq, ErrorOr<Deleted>>
{
    private readonly ApiClientProvider _api;
    public DeletePaymentSpecReqHlr(ApiClientProvider api) => _api = api;

    public async Task<ErrorOr<Deleted>> Handle(DeletePaymentSpecReq request, CancellationToken cancellationToken)
    {
        var uri = $"{_api.ApiV1PaymentSpecsUri}/{request.Id}";
        var responseMessage = await _api.Client.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);

        return responseMessage.StatusCode switch
        {
            HttpStatusCode.NoContent => Result.Deleted,
            HttpStatusCode.NotFound => Error.NotFound(),
            _ => Error.Unexpected(),
        };
    }
}
