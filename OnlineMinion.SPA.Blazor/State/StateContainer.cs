namespace OnlineMinion.SPA.Blazor.State;

public class StateContainer
{
    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnChange?.Invoke();
        }
    }

    public event Action? OnChange;
}
