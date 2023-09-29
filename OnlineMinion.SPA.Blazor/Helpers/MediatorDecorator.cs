using MediatR;
using OnlineMinion.Application.RequestValidation;

namespace OnlineMinion.SPA.Blazor.Helpers;

public class MediatorDecorator(ISender sender) : IAsyncValidatorSender
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> rq, CancellationToken ct = default) =>
        sender.Send(rq, ct);
}
