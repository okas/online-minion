namespace OnlineMinion.Web.Helpers;

public static class AsyncStreamRenderingHelpers
{
    /// <summary>
    ///     Pulls items from the given <paramref name="sourceStream" /> and adds them to the <paramref name="targetList" />.
    ///     <br />
    ///     Optionally invokes the <paramref name="afterEachPull" /> action after each pull after specified
    ///     <paramref name="timeoutMilliseconds" />.
    /// </summary>
    /// <param name="sourceStream">Async Enumerable stream to pull data from.</param>
    /// <param name="targetList">List to add pulled items.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <param name="afterEachPull">Optional action to call after specified timeout.</param>
    /// <param name="timeoutMilliseconds">
    ///     Optional timeout to decide, whether after each pull the action <paramref name="afterEachPull" /> needs to be
    ///     called. For UX purposes, e.g. for notification.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     When <paramref name="sourceStream" /> and/or <paramref name="targetList" /> is null.
    /// </exception>
    public static ValueTask PullItemsFromStream<TItem>(
        this IAsyncEnumerable<TItem> sourceStream,
        IList<TItem>                 targetList,
        CancellationToken            ct,
        Action?                      afterEachPull       = null,
        double                       timeoutMilliseconds = 20D
    )
    {
        ArgumentNullException.ThrowIfNull(sourceStream);
        ArgumentNullException.ThrowIfNull(targetList);

        return PullItemsInternalAsync(sourceStream, targetList.Add, afterEachPull, timeoutMilliseconds, ct);
    }

    /// <summary>
    ///     Pulls each item from the given <paramref name="sourceStream" /> using the <paramref name="pullAction" />.
    ///     <br />
    ///     Optionally invokes the <paramref name="afterEachPull" /> action after each pull after specified
    ///     <paramref name="timeoutMilliseconds" />.
    /// </summary>
    /// <param name="sourceStream">Async Enumerable stream to pull data from.</param>
    /// <param name="pullAction">Action to run when item is read from stream, takes item as argument.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <param name="afterEachPull">Optional action to call after specified timeout.</param>
    /// <param name="timeoutMilliseconds">
    ///     Optional timeout to decide, whether after each pull the action <paramref name="afterEachPull" /> needs to be
    ///     called. For UX purposes, e.g. for notification.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     When <paramref name="sourceStream" /> and/or <paramref name="pullAction" /> is null.
    /// </exception>
    public static ValueTask PullItemsFromStream<TSource>(
        this IAsyncEnumerable<TSource> sourceStream,
        Action<TSource>                pullAction,
        CancellationToken              ct,
        Action?                        afterEachPull       = null,
        double                         timeoutMilliseconds = 20D
    )
    {
        ArgumentNullException.ThrowIfNull(sourceStream);
        ArgumentNullException.ThrowIfNull(pullAction);

        return PullItemsInternalAsync(sourceStream, pullAction, afterEachPull, timeoutMilliseconds, ct);
    }

    private static async ValueTask PullItemsInternalAsync<TSource>(
        IAsyncEnumerable<TSource> sourceStream,
        Action<TSource>           pullAction,
        Action?                   afterEachPullAction,
        double                    timeoutMilliseconds,
        CancellationToken         ct
    )
    {
        var lastRender = DateTime.UtcNow;

        await foreach (var item in sourceStream.WithCancellation(ct).ConfigureAwait(false))
        {
            pullAction.Invoke(item);

            if (afterEachPullAction is not null)
            {
                lastRender = RunAfterPullAction(afterEachPullAction, timeoutMilliseconds, lastRender);
            }
        }
    }

    private static DateTime RunAfterPullAction(Action action, double timeoutMilliseconds, DateTime lastRender)
    {
        var utcNow = DateTime.UtcNow;

        var diffInMs = (utcNow - lastRender).TotalMilliseconds;
        if (diffInMs >= timeoutMilliseconds)
        {
            action.Invoke();
        }

        return utcNow;
    }
}
