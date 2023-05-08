namespace OnlineMinion.Contracts.Responses;

public record struct BasePagedResult<TResult>(IList<TResult> Result, PagingMetaInfo Paging);
