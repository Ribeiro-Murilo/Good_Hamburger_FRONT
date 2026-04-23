namespace GoodHamburgerFront.Services;

public class LoadingService
{
    private int _activeRequests = 0;

    public bool IsLoading => _activeRequests > 0;

    public event Action? OnChange;

    public void Increment()
    {
        Interlocked.Increment(ref _activeRequests);
        OnChange?.Invoke();
    }

    public void Decrement()
    {
        if (Interlocked.Decrement(ref _activeRequests) < 0)
            Interlocked.Exchange(ref _activeRequests, 0);
        OnChange?.Invoke();
    }
}
