namespace OnlineMinion.Web.Helpers;

public static class AsyncStreamRenderingHelpers
{
    /// <summary>
    ///     Pulls items from the given <paramref name="sourceStream" /> and adds them to the <paramref name="targetList" />.
    ///     <br />
    ///     Optionally invokes the <paramref name="afterEachPull" /> action after each pull after specified
    ///     <paramref name="timeoutMilliseconds" />.
    /// </summary>
    /// <param name="sourceStream">Async Enumerable stream to pull data.</param>
    /// <param name="targetList">List to add pulled items.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <param name="afterEachPull">Optional action to call after specified timeout.</param>
    /// <param name="timeoutMilliseconds">
    ///     Optional timeout to decide whether after each action <paramref name="afterEachPull" /> needs to be called. For UX
    ///     purposes, e.g. to notify that new item has been pulled into <paramref name="targetList" />.
    /// </param>
    /// <exception cref="ArgumentNullException">When sourceStream and/or <paramref name="targetList" /> is null.</exception>
    public static async ValueTask PullItemsFromStream<TItem>(
        this IAsyncEnumerable<TItem> sourceStream,
        IList<TItem>                 targetList,
        CancellationToken            ct,
        Action?                      afterEachPull       = null,
        double                       timeoutMilliseconds = 20D
    )
    {
        ArgumentNullException.ThrowIfNull(sourceStream);
        ArgumentNullException.ThrowIfNull(targetList);

        var lastRender = DateTime.UtcNow;

        await foreach (var item in sourceStream.WithCancellation(ct).ConfigureAwait(false))
        {
            targetList.Add(item);

            if (afterEachPull is null)
            {
                continue;
            }

            var pointInTime = DateTime.UtcNow;

            var diffInMs = (pointInTime - lastRender).TotalMilliseconds;
            if (diffInMs >= timeoutMilliseconds)
            {
                afterEachPull.Invoke();
            }

            lastRender = pointInTime;
        }
    }
}
