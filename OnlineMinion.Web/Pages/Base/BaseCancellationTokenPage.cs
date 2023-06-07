using Microsoft.AspNetCore.Components;

namespace OnlineMinion.Web.Pages.Base;

public abstract class BaseCancellationTokenPage : ComponentBase, IAsyncDisposable
{
    protected readonly CancellationTokenSource CancellationTokenSource;
    protected readonly CancellationToken CT;
    private bool _disposed;

    protected BaseCancellationTokenPage()
    {
        // TODO Can it be set up & injected from DI?
        CancellationTokenSource = new();
        CT = CancellationTokenSource.Token;
    }

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
