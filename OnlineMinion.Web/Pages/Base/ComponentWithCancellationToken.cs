using Microsoft.AspNetCore.Components;

namespace OnlineMinion.Web.Pages.Base;

public abstract class ComponentWithCancellationToken : ComponentBase, IAsyncDisposable
{
    private bool _isDisposed;

    protected CancellationTokenSource? CancellationTokenSource { get; private set; }

    protected CancellationToken CT => (CancellationTokenSource ??= new()).Token;

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        if (CancellationTokenSource is not null)
        {
            await CancellationTokenSource.CancelAsync();
            CancellationTokenSource.Dispose();
            CancellationTokenSource = null;
        }

        GC.SuppressFinalize(this);

        _isDisposed = true;
    }
}
