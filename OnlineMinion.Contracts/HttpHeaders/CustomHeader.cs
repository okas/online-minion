namespace OnlineMinion.Contracts.HttpHeaders;

public static class CustomHeaderNames
{
    public const string PagingTotalItems = $"Paging-{nameof(PagingMetaInfo.TotalItems)}";
    public const string PagingSize = $"Paging-{nameof(PagingMetaInfo.Size)}";
    public const string PagingPages = $"Paging-{nameof(PagingMetaInfo.Pages)}";
}
