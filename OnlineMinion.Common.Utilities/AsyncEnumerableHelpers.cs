using System.Runtime.CompilerServices;

namespace OnlineMinion.Common.Utilities;

public static class AsyncEnumerableHelpers
{
    public static async IAsyncEnumerable<TItem> ToDelayedAsyncEnumerable<TItem>(
        this IAsyncEnumerable<TItem>               source,
        double                                     milliseconds,
        [EnumeratorCancellation] CancellationToken ct = default
    )
    {
        await foreach (var item in source.WithCancellation(ct).ConfigureAwait(false))
        {
            yield return item;
            await Task.Delay(TimeSpan.FromMilliseconds(milliseconds), ct).ConfigureAwait(false);
        }
    }

    public static IAsyncEnumerable<TItem> ToDelayedAsyncEnumerable<TItem>(
        this IEnumerable<TItem> source,
        double                  milliseconds,
        CancellationToken       ct = default
    ) => ToDelayedAsyncEnumerable(source.ToAsyncEnumerable(), milliseconds, ct);
}
