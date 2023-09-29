using ErrorOr;
using MediatR;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal interface IApiClientRequestHandler<in TRequest, TResult>
    : IRequestHandler<TRequest, ErrorOr<TResult>>
    where TRequest : IRequest<ErrorOr<TResult>>
{
    protected Uri BuildUri(TRequest rq);
}
