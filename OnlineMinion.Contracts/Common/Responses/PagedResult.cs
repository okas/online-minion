namespace OnlineMinion.Contracts.Common.Responses;

public record struct PagedResult<TResult>(IAsyncEnumerable<TResult> Result, PagingMetaInfo Paging);
