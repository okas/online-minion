using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace OnlineMinion.Presentation.Utilities;

public static class AsyncEnumerableHelpers
{
    [System.Diagnostics.Contracts.Pure]
    [LinqTunnel]
    public static async IAsyncEnumerable<TItem> ToDelayedAsyncEnumerable<TItem>(
        this IAsyncEnumerable<TItem>               source,
        double                                     milliseconds,
        [EnumeratorCancellation] CancellationToken ct = default
    )
    {
        var timeSpan = TimeSpan.FromMilliseconds(milliseconds);

        await foreach (var item in source.WithCancellation(ct).ConfigureAwait(false))
        {
            yield return item;
            await Task.Delay(timeSpan, ct).ConfigureAwait(false);
        }
    }

    [System.Diagnostics.Contracts.Pure]
    [LinqTunnel]
    public static IAsyncEnumerable<TItem> ToDelayedAsyncEnumerable<TItem>(
        this IEnumerable<TItem> source,
        double                  milliseconds,
        CancellationToken       ct = default
    ) => ToDelayedAsyncEnumerable(source.ToAsyncEnumerable(), milliseconds, ct);
}
