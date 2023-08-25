using System.Net;
using ErrorOr;
using MediatR;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.Common.Handlers;

internal abstract class BaseCheckUniqueReqHlr<TRequest> : IRequestHandler<TRequest, ErrorOr<Success>>
    where TRequest : IRequest<ErrorOr<Success>>
{
    protected readonly ApiClientProvider Api;
    protected BaseCheckUniqueReqHlr(ApiClientProvider api) => Api = api;

    public async Task<ErrorOr<Success>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUrl(rq);

        var message = new HttpRequestMessage(HttpMethod.Head, uri);

        var result = await Api.Client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        return result.StatusCode switch
        {
            HttpStatusCode.NoContent => Result.Success,
            HttpStatusCode.Conflict => Error.Conflict(),
            _ => Error.Unexpected(),
        };
    }

    protected abstract Uri BuildUrl(TRequest rq);
}
