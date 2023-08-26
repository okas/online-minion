using OnlineMinion.Data;

namespace OnlineMinion.RestApi.Shared.Requests;

/// <summary>
///     These request handlers must be defined per type in program startup, because MS DI cannot resolve them
///     automatically.
/// </summary>
/// <param name="PageSize"></param>
/// <typeparam name="TEntity"></typeparam>
internal record GetPagingMetaInfoReq<TEntity>(int PageSize = 10) : IPagedResourceRequest<TEntity>
    where TEntity : BaseEntity;
