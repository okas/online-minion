namespace OnlineMinion.Contracts.AppMessaging;

public interface IPagedRequest
{
    int Page { get; }

    int PageSize { get; }
}
