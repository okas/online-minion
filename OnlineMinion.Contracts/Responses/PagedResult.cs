namespace OnlineMinion.Contracts.Responses;

public record struct PagedResult<TResult>(IAsyncEnumerable<TResult> Result, PagingMetaInfo Paging);
