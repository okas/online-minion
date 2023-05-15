using MediatR;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public record GetPagingMetaInfoReq<TEntity>(int PageSize = 10) : IRequest<PagingMetaInfo> where TEntity : BaseEntity;
