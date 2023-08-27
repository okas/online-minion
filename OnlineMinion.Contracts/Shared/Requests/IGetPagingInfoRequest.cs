using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface IGetPagingInfoRequest : IRequest<ErrorOr<PagingMetaInfo>>
{
    int PageSize { get; }
}
