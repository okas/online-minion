using MediatR;

namespace OnlineMinion.Contracts.Queries;

public record GetPagingMetaInfoQry<TEntity>(int PageSize = 10) : IRequest<PagingMetaInfo> where TEntity : BaseEntity;
