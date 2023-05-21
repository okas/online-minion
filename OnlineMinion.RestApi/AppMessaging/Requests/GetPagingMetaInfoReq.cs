using MediatR;
using OnlineMinion.Contracts;
using OnlineMinion.Data.BaseEntities;

namespace OnlineMinion.RestApi.AppMessaging.Requests;

public record GetPagingMetaInfoReq<TEntity>(int PageSize = 10) : IRequest<PagingMetaInfo> where TEntity : BaseEntity;
