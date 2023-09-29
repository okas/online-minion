using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineMinion.Application.Contracts;

/// <param name="Size">Size of page.</param>
/// <param name="Page">Order number of page.</param>
public abstract record BasePagingParams(
    int                                     Page = BasePagingParams.FirstPage,
    [AllowedValues(5, 10, 20, 50, 100)] int Size = BasePagingParams.DefaultSize
) : IPagingInfo
{
    [JsonIgnore] public const int FirstPage = 1;

    [JsonIgnore] public const int DefaultSize = 10;

    [JsonIgnore]
    public static IEnumerable<int> AllowedSizes { get; } = new[] { 5, 10, 20, 50, 100, };
}
