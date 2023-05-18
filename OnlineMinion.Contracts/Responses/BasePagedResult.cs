namespace OnlineMinion.Contracts.Responses;

public record struct BasePagedResult<TResult>(IAsyncEnumerable<TResult> Result, PagingMetaInfo Paging);
