using ErrorOr;
using MediatR;

namespace OnlineMinion.Application.Contracts.Shared.Requests;

public interface IGetPagingInfoRequest : IRequest<ErrorOr<PagingMetaInfo>>
{
    int PageSize { get; }
}
