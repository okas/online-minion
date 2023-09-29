using System.Globalization;
using System.Net;
using ErrorOr;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseDeleteModelReqHlr<TRequest>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<TRequest, Deleted>
    where TRequest : IDeleteByIdCommand
{
    public async Task<ErrorOr<Deleted>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        var responseMessage = await apiClient.DeleteAsync(uri, ct).ConfigureAwait(false);

        return HandleResponse(responseMessage);
    }

    public virtual Uri BuildUri(TRequest request) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{resource}/{request.Id}"
        ),
        UriKind.RelativeOrAbsolute
    );

    private static ErrorOr<Deleted> HandleResponse(HttpResponseMessage response) => response.StatusCode switch
    {
        HttpStatusCode.NoContent => Result.Deleted,
        HttpStatusCode.NotFound => Error.NotFound(),
        _ => Error.Unexpected(),
    };
}
