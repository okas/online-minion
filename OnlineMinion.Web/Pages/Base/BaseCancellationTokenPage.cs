using Microsoft.AspNetCore.Components;

namespace OnlineMinion.Web.Pages.Base;

public abstract class BaseCancellationTokenPage : ComponentBase, IAsyncDisposable
{
    protected readonly CancellationTokenSource CancellationTokenSource;
    private bool _disposed;

    protected BaseCancellationTokenPage() => CancellationTokenSource = new();

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (!_disposed)
        {
            _disposed = true;
            await CancellationTokenSource.CancelAsync();
            CancellationTokenSource.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}
