using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface IGetPagingInfoReq : IRequest<ErrorOr<int>>
{
    int PageSize { get; }
}