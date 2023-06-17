using Microsoft.AspNetCore.Components;

namespace OnlineMinion.Web.Components.Base;

public abstract class ComponentWithCancellationToken : ComponentBase, IAsyncDisposable
{
    protected CancellationTokenSource? CancellationTokenSource { get; private set; }

    protected CancellationToken CT => (CancellationTokenSource ??= new()).Token;

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (CancellationTokenSource is not null)
        {
            await CancellationTokenSource.CancelAsync();
            CancellationTokenSource.Dispose();
            CancellationTokenSource = null;
        }

        GC.SuppressFinalize(this);
    }
}
