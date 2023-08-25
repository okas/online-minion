using MediatR;
using OnlineMinion.Contracts;
using OnlineMinion.Data.BaseEntities;

namespace OnlineMinion.RestApi.Common.Requests;

/// <summary>
///     With the help of covariance, it helps do abstract the request handler from the entity type. Also helps to
///     generalize paging metainfo retrieval code for each database entity type.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IPagedResourceRequest<out TEntity> : IRequest<PagingMetaInfo>
    where TEntity : BaseEntity
{
    int PageSize { get; }
}
