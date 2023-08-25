using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Common.Requests;

public interface IGetPagingInfoReq : IRequest<ErrorOr<int>>
{
    int PageSize { get; }
}
