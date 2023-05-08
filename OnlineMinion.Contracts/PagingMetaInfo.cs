using System.Text.Json.Serialization;

namespace OnlineMinion.Contracts;

public readonly record struct PagingMetaInfo(
    int TotalItems,
    int Size    = PagingMetaInfo.DefaultSize,
    int Current = PagingMetaInfo.DefaultCurrent
)
{
    [JsonIgnore] public const int DefaultSize = 10;

    [JsonIgnore] public const int DefaultCurrent = 1;

    [JsonIgnore] public const int First = 1;

    [JsonIgnore]
    public int Pages => (int)Math.Ceiling((decimal)TotalItems / SanitizeSize(Size));

    [JsonIgnore]
    public int ItemsOffset => (Current > 1 ? Current - 1 : 0) * SanitizeSize(Size);

    [JsonIgnore]
    public int Previous => SanitizePage(Current - 1);

    [JsonIgnore]
    public int Next => SanitizePage(Current + 1);

    public int SanitizePage(int page) => page >= First && page <= Pages ? page : Current;

    public int GetNewCurrentBySize(int size) => (int)Math.Ceiling((decimal)(ItemsOffset + 1) / SanitizeSize(size));

    private static int SanitizeSize(int size) => size > 0 ? size : DefaultSize;
}
