using ErrorOr;
using MediatR;
using OnlineMinion.Application.Contracts.Shared.Responses;

namespace OnlineMinion.Application.Contracts.Shared.Requests;

public interface IGetStreamedPagedRequest<TResponse>
    : IRequest<ErrorOr<PagedStreamResult<TResponse>>>, IQueryParameters;
