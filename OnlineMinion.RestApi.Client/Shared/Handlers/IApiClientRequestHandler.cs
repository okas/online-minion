using ErrorOr;
using MediatR;
using OnlineMinion.Common;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal interface IApiClientRequestHandler<in TRequest, TResult> : IErrorOrRequestHandler<TRequest, TResult>
    where TRequest : IRequest<ErrorOr<TResult>>
{
    protected Uri BuildUri(TRequest rq);
}
