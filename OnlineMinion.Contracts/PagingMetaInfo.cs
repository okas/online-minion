using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace OnlineMinion.Contracts;

[StructLayout(LayoutKind.Auto)]
public readonly record struct PagingMetaInfo(
    int TotalItems,
    int Size = PagingMetaInfo.DefaultSize,
    int Page = PagingMetaInfo.DefaultCurrent
) : IPagingInfo
{
    [JsonIgnore] public const int DefaultSize = 10;

    [JsonIgnore] public const int DefaultCurrent = 1;

    [JsonIgnore] public const int First = 1;

    [JsonIgnore]
    public int Pages => (int)Math.Ceiling((decimal)TotalItems / SanitizeSize(Size));

    [JsonIgnore]
    public int ItemsOffset => (Page > 1 ? Page - 1 : 0) * SanitizeSize(Size);

    [JsonIgnore]
    public int Previous => SanitizePage(Page - 1);

    [JsonIgnore]
    public int Next => SanitizePage(Page + 1);

    public int SanitizePage(int page) => page >= First && page <= Pages ? page : Page;

    public int GetNewCurrentBySize(int size) => (int)Math.Ceiling((decimal)(ItemsOffset + 1) / SanitizeSize(size));

    private static int SanitizeSize(int size) => size > 0 ? size : DefaultSize;
}
