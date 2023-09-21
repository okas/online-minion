namespace OnlineMinion.Contracts;

public static class CustomHeaderNames
{
    public const string PagingRows = $"Paging-{nameof(PagingMetaInfo.Rows)}";
    public const string PagingSize = $"Paging-{nameof(PagingMetaInfo.Size)}";
    public const string PagingPages = $"Paging-{nameof(PagingMetaInfo.Pages)}";
    public const string ApiVersion = "api-ver";
}
