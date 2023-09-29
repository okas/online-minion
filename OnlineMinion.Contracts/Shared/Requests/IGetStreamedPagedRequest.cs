using ErrorOr;
using MediatR;
using OnlineMinion.Contracts.Shared.Responses;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface IGetStreamedPagedRequest<TResponse>
    : IRequest<ErrorOr<PagedStreamResult<TResponse>>>, IFullQueryParams;
