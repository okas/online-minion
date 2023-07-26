using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace OnlineMinion.Contracts;

/// <summary>
///     Carries paging metadata, can be used in the "envelope" types, most often  used in request responses.
/// </summary>
/// <param name="Rows">Total rows in resource.</param>
[StructLayout(LayoutKind.Auto)]
public sealed record PagingMetaInfo(int Rows) : BasePagingParams
{
    public PagingMetaInfo(int rows, int size, int page) : this(rows) => (Size, Page) = (size, page);

    [JsonIgnore]
    public int Pages => (int)Math.Ceiling((decimal)Rows / (Size > 0 ? Size : DefaultSize));
}
