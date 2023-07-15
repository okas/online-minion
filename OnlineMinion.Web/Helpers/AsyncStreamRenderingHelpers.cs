namespace OnlineMinion.Web.Helpers;

public static class AsyncStreamRenderingHelpers
{
    /// <summary>
    ///     Pulls items from the given <paramref name="sourceStream" /> and adds them to the <paramref name="targetList" />.
    ///     <br />
    ///     Optionally invokes the <paramref name="afterEachPull" /> action after each pull.
    /// </summary>
    /// <exception cref="ArgumentNullException"> When sourceStream and/or targetList is null.</exception>
    public static async ValueTask PullItemsFromStream<TItem>(
        this IAsyncEnumerable<TItem> sourceStream,
        IList<TItem>                 targetList,
        Action?                      afterEachPull,
        CancellationToken            ct
    )
    {
        ArgumentNullException.ThrowIfNull(sourceStream);
        ArgumentNullException.ThrowIfNull(targetList);

        var lastRender = DateTime.UtcNow;

        await foreach (var item in sourceStream.WithCancellation(ct).ConfigureAwait(false))
        {
            targetList.Add(item);

            var pointInTime = DateTime.UtcNow;
            if ((pointInTime - lastRender).TotalMilliseconds >= 20D)
            {
                afterEachPull?.Invoke();
            }

            lastRender = pointInTime;
        }
    }
}
