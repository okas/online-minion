namespace OnlineMinion.Contracts.HttpHeaders;

public static class CustomHeaderNames
{
    private const string Prefix = "Paging";

    public const string PagingTotalItems = $"{Prefix}-{nameof(PagingMetaInfo.TotalItems)}";
    public const string PagingSize = $"{Prefix}-{nameof(PagingMetaInfo.Size)}";
    public const string PagingPages = $"{Prefix}-{nameof(PagingMetaInfo.Pages)}";
}
