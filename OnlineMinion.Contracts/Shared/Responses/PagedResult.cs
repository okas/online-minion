namespace OnlineMinion.Contracts.Shared.Responses;

public record struct PagedResult<TResult>(IAsyncEnumerable<TResult> Result, PagingMetaInfo Paging);
