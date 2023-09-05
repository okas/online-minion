namespace OnlineMinion.Contracts.Shared.Responses;

public record struct PagedStreamResult<TResult>(IAsyncEnumerable<TResult> StreamResult, PagingMetaInfo Paging);
